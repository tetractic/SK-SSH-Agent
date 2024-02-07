// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using SKSshAgent.Ssh;
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Win32.Foundation;
using static Windows.Win32.PInvoke;

namespace SKSshAgent
{
    internal abstract class SshAgentPipe
    {
        private const int _maxMessageLength = 256 * 1024;

        private readonly KeyListForm _form;

        private readonly CancellationTokenSource _cts = new();

        private readonly HashSet<Task> _connectionTasks = new();

        private readonly TaskCompletionSource _stoppedTcs = new();

        private volatile SshAgentPipeStatus _status;

        private Task? _acceptTask;

        private Task? _stopTask;

        private bool _acceptCompleted;

        public SshAgentPipe(KeyListForm form)
        {
            _form = form;
        }

        public event StatusChangedEventHandler? StatusChanged;

        public abstract string PipeName { get; }

        public SshAgentPipeStatus Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;

                _status = value;

                StatusChanged?.Invoke(this, value);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            lock (_connectionTasks)
            {
                if (_acceptTask == null)
                {
                    Status = SshAgentPipeStatus.Starting;

                    _acceptTask = Task.Run(() => AcceptLoopAsync(_cts.Token), cancellationToken);
                }
            }

            return Task.CompletedTask;
        }

        /// <exception cref="InvalidOperationException"/>
        public Task StopAsync()
        {
            lock (_connectionTasks)
            {
                if (_acceptTask == null)
                    throw new InvalidOperationException("Not started.");

                _cts.Cancel();

                if (_stopTask == null)
                    _stopTask = Core();
            }

            return _stopTask;

            async Task Core()
            {
                try
                {
                    await _acceptTask.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                lock (_connectionTasks)
                {
                    _acceptCompleted = true;

                    if (_connectionTasks.Count == 0)
                        _stoppedTcs.SetResult();
                }

                await _stoppedTcs.Task.ConfigureAwait(false);

                Status = SshAgentPipeStatus.Stopped;
            }
        }

        protected static string GetPipePath(string pipeName)
        {
            return $@"\\.\pipe\" + pipeName;
        }

        private async Task AcceptLoopAsync(CancellationToken cancellationToken)
        {
            string pipeName = PipeName;

            string pipePath = GetPipePath(pipeName);

            while (File.Exists(pipePath))
            {
                Status = SshAgentPipeStatus.AlreadyInUse;

                await Task.Delay(100, cancellationToken).ConfigureAwait(false);
            }

            Status = SshAgentPipeStatus.RunningIdle;

            while (!cancellationToken.IsCancellationRequested)
            {
                NamedPipeServerStream stream;
                try
                {
                    stream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, -1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly);
                }
                catch (IOException)
                {
                    try
                    {
                        await Task.Delay(100, cancellationToken).ConfigureAwait(false);

                        continue;
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }

                try
                {
                    await stream.WaitForConnectionAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    await stream.DisposeAsync().ConfigureAwait(false);

                    break;
                }

                var task = HandleConnectionAsync(stream, cancellationToken);

                lock (_connectionTasks)
                {
                    _ = _connectionTasks.Add(task);

                    Status = SshAgentPipeStatus.RunningBusy;
                }

                _ = task.ContinueWith(task =>
                {
                    lock (_connectionTasks)
                    {
                        _ = _connectionTasks.Remove(task);

                        if (_connectionTasks.Count == 0)
                        {
                            if (Status == SshAgentPipeStatus.RunningBusy)
                                Status = SshAgentPipeStatus.RunningIdle;

                            if (_acceptCompleted)
                                _stoppedTcs.SetResult();
                        }
                    }

                    try
                    {
                        // Propagate exception if there was one.
                        task.GetAwaiter().GetResult();
                    }
                    catch (OperationCanceledException)
                    {
                        // Expected.
                    }
                }, CancellationToken.None);
            }
        }

        private async Task HandleConnectionAsync(Stream stream, CancellationToken cancellationToken)
        {
            try
            {
                await using (stream.ConfigureAwait(false))
                {
                    byte[] lengthBuffer = new byte[4];
                    var buffer = new ArrayBufferWriter<byte>(5);

                    while (true)
                    {
                        if (await ReadAsync(stream, lengthBuffer, cancellationToken).ConfigureAwait(false) < 4)
                            return;

                        int length = BinaryPrimitives.ReadInt32BigEndian(lengthBuffer);
                        if (length < 1 || length > _maxMessageLength)
                            return;

                        if (await ReadAsync(stream, buffer.GetMemory(length).Slice(0, length), cancellationToken).ConfigureAwait(false) < length)
                            return;
                        buffer.Advance(length);

                        await HandleMessageAsync(buffer, cancellationToken).ConfigureAwait(false);

                        BinaryPrimitives.WriteInt32BigEndian(lengthBuffer, buffer.WrittenCount);

                        await stream.WriteAsync(lengthBuffer, cancellationToken).ConfigureAwait(false);

                        await stream.WriteAsync(buffer.WrittenMemory, cancellationToken).ConfigureAwait(false);

                        buffer.Clear();
                    }
                }
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex);
            }

            static async Task<int> ReadAsync(Stream stream, Memory<byte> buffer, CancellationToken cancellationToken)
            {
                int totalRead = 0;
                while (buffer.Length > 0)
                {
                    int read = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
                    if (read == 0)
                        break;
                    buffer = buffer.Slice(read);
                    totalRead += read;
                }
                return totalRead;
            }
        }

        private async Task HandleMessageAsync(ArrayBufferWriter<byte> buffer, CancellationToken cancellationToken)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-agent.c#L1603

            var messageType = (MessageType)buffer.WrittenSpan[0];

            switch (messageType)
            {
                case MessageType.SSH_AGENTC_REQUEST_IDENTITIES:
                {
                    HandleRequestIdentities(buffer);
                    break;
                }
                case MessageType.SSH_AGENTC_SIGN_REQUEST:
                {
                    await HandleSignRequestAsync(buffer, cancellationToken).ConfigureAwait(false);
                    break;
                }
                default:
                    WriteSimpleResponse(buffer, MessageType.SSH_AGENT_FAILURE);
                    break;
            }
        }

        private static void WriteSimpleResponse(ArrayBufferWriter<byte> buffer, MessageType messageType)
        {
            buffer.Clear();

            var span = buffer.GetSpan(1);
            span[0] = (byte)messageType;
            buffer.Advance(1);
        }

        private static void HandleRequestIdentities(ArrayBufferWriter<byte> buffer)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-agent.c#L512

            var items = KeyList.Instance.GetAllKeys();

            buffer.Clear();

            buffer.GetSpan(1)[0] = (byte)MessageType.SSH_AGENT_IDENTITIES_ANSWER;
            buffer.Advance(1);

            var keyBuffer = new ArrayBufferWriter<byte>();

            var writer = new SshWireWriter(buffer);
            writer.WriteUInt32((uint)items.Length);
            foreach (var item in items)
            {
                keyBuffer.Clear();
                var keyWriter = new SshWireWriter(keyBuffer);
                item.Key.WritePublicKeyTo(ref keyWriter);
                keyWriter.Flush();

                writer.WriteByteString(keyBuffer.WrittenSpan);
                writer.WriteString(item.Comment);
            }
            writer.Flush();
        }

        private async Task HandleSignRequestAsync(ArrayBufferWriter<byte> buffer, CancellationToken cancellationToken)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-agent.c#L722

            var contents = buffer.WrittenMemory.Slice(1);

            SshKey? key;
            byte[]? data;
            SignatureFlags signatureFlags;

            if (!TryParseRequest(contents, out key, out data, out signatureFlags))
            {
                WriteSimpleResponse(buffer, MessageType.SSH_AGENT_FAILURE);
                return;
            }

            if (!KeyList.Instance.TryGetKey(key, out key, out _) ||
                signatureFlags != SignatureFlags.None)
            {
                WriteSimpleResponse(buffer, MessageType.SSH_AGENT_FAILURE);
                return;
            }

            bool useConfirmed = !Settings.ConfirmEachKeyUse;

            if (key.EncryptedPrivateKey != null)
            {
                var privateKey = await _form.InvokeAsync(() => _form.DecryptPrivateKeyAsync(key, background: true, cancellationToken)).ConfigureAwait(false);
                if (privateKey == null)
                {
                    WriteSimpleResponse(buffer, MessageType.SSH_AGENT_FAILURE);
                    return;
                }

                useConfirmed = true;

                key = privateKey;
            }

            SshSignature signature;

            switch (key.KeyTypeInfo.KeyType)
            {
                case SshKeyType.Ecdsa:
                case SshKeyType.Ed25519:
                {
                    if (!useConfirmed)
                        useConfirmed = await _form.InvokeAsync(() => _form.ConfirmKeyUse(key, background: true, cancellationToken)).ConfigureAwait(false);

                    if (!useConfirmed)
                    {
                        WriteSimpleResponse(buffer, MessageType.SSH_AGENT_FAILURE);
                        return;
                    }

                    switch (key.KeyTypeInfo.KeyType)
                    {
                        case SshKeyType.Ecdsa:
                        {
                            var ecdsaKey = (SshEcdsaKey)key;

                            signature = ecdsaKey.Sign(data);
                            break;
                        }
                        case SshKeyType.Ed25519:
                        {
                            var ed25519Key = (SshEd25519Key)key;

                            signature = ed25519Key.Sign(data);
                            break;
                        }
                        default:
                            throw new UnreachableException();
                    }
                    break;
                }
                case SshKeyType.OpenSshEcdsaSK:
                case SshKeyType.OpenSshEd25519SK:
                {
                    if (!(WebAuthnApi.Version >= WEBAUTHN_API_VERSION_1))
                    {
                        WriteSimpleResponse(buffer, MessageType.SSH_AGENT_FAILURE);
                        return;
                    }

                    var webAuthnKey = key.ToWebAuthnKey();

                    string rpId;
                    ShieldedImmutableBuffer keyHandle;
                    OpenSshSKFlags keyFlags;
                    byte[] challenge;

                    switch (key.KeyTypeInfo.KeyType)
                    {
                        case SshKeyType.OpenSshEcdsaSK:
                        {
                            var ecdsaSKKey = (OpenSshEcdsaSKKey)key;

                            rpId = Encoding.UTF8.GetString(ecdsaSKKey.Application.AsSpan());
                            keyHandle = ecdsaSKKey.KeyHandle;
                            keyFlags = ecdsaSKKey.Flags;
                            challenge = data;
                            break;
                        }
                        case SshKeyType.OpenSshEd25519SK:
                        {
                            var ed25519SKKey = (OpenSshEd25519SKKey)key;

                            rpId = Encoding.UTF8.GetString(ed25519SKKey.Application.AsSpan());
                            keyHandle = ed25519SKKey.KeyHandle;
                            keyFlags = ed25519SKKey.Flags;
                            challenge = data;
                            break;
                        }
                        default:
                            throw new UnreachableException();
                    }

                    // For "webauthn" key types (ex. "webauthn-sk-ecdsa-sha2-nistp256@openssh.com"),
                    // challenge would be constructed per [1].
                    // [1] https://www.w3.org/TR/webauthn/#clientdatajson-serialization

                    try
                    {
                        // Key use is always confirmed by Windows WebAuthn since it doesn't support "no-touch-required".

                        WebAuthnApi.GetAssertionResult result;

                        (var form, var handle) = await _form.InvokeAsync(() =>
                        {
                            var form = new WebAuthnKeyUseConfirmationForm();
                            form.Fingerprint = "SHA256:" + Convert.ToBase64String(key.GetSha256Fingerprint()).TrimEnd('=');
                            form.Text += " — " + _form.Text;
                            form.Show();
                            return (form, form.Handle);
                        }).ConfigureAwait(false);

                        try
                        {
                            var hWnd = new HWND(handle);
                            using (var keyHandleUnshieldScope = keyHandle.Unshield())
                                result = WebAuthnApi.GetAssertion(hWnd, webAuthnKey, rpId, keyHandleUnshieldScope.UnshieldedSpan, keyFlags, challenge, cancellationToken);
                        }
                        finally
                        {
                            await form.InvokeAsync(() => form.Close()).ConfigureAwait(false);
                        }

                        var webAuthnSignature = result.Signature;
                        byte flags = (byte)result.AuthenticatorData.Flags;
                        uint counter = result.AuthenticatorData.SignCount;

                        switch (webAuthnSignature.Algorithm)
                        {
                            case CoseAlgorithm.ES256:
                            case CoseAlgorithm.ES384:
                            case CoseAlgorithm.ES512:
                            {
                                var webAuthnEcdsaSignature = (CoseEcdsaSignature)webAuthnSignature;

                                signature = webAuthnEcdsaSignature.ToOpenSshSignature(flags, counter);
                                break;
                            }
                            case CoseAlgorithm.EdDsa:
                            {
                                var webAuthnEdDsaSignature = (CoseEdDsaSignature)webAuthnSignature;

                                signature = webAuthnEdDsaSignature.ToOpenSshSignature(flags, counter);
                                break;
                            }
                            default:
                                throw new UnreachableException();
                        }
                    }
                    catch (Exception ex)
                        when (ex is InvalidDataException ||
                              ex is OperationCanceledException)
                    {
                        WriteSimpleResponse(buffer, MessageType.SSH_AGENT_FAILURE);
                        return;
                    }
                    break;
                }
                default:
                    throw new UnreachableException();
            }

            WriteResponse(buffer, signature);

            static bool TryParseRequest(ReadOnlyMemory<byte> contents, [MaybeNullWhen(false)] out SshKey key, [MaybeNullWhen(false)] out byte[] data, out SignatureFlags signatureFlags)
            {
                try
                {
                    var reader = new SshWireReader(contents.Span);

                    ReadOnlySpan<byte> keyBytes;
                    if (!reader.TryReadByteString(out keyBytes) ||
                        !reader.TryReadByteString(out var tempData) ||
                        !reader.TryReadUInt32(out uint tempSignatureFlags))
                    {
                        key = default;
                        data = default;
                        signatureFlags = default;
                        return false;
                    }

                    var keyReader = new SshWireReader(keyBytes);
                    key = SshKey.ReadPublicKey(ref keyReader);

                    data = tempData.ToArray();

                    signatureFlags = (SignatureFlags)tempSignatureFlags;

                    return true;
                }
                catch (Exception ex)
                    when (ex is SshWireContentException ||
                          ex is InvalidDataException ||
                          ex is NotSupportedException)
                {
                    key = default;
                    data = default;
                    signatureFlags = default;
                    return false;
                }
            }

            static void WriteResponse(ArrayBufferWriter<byte> buffer, SshSignature signature)
            {
                buffer.Clear();

                buffer.GetSpan(1)[0] = (byte)MessageType.SSH_AGENT_SIGN_RESPONSE;
                buffer.Advance(1);

                var signatureBuffer = new ArrayBufferWriter<byte>();
                var signatureWriter = new SshWireWriter(signatureBuffer);
                signature.WriteTo(ref signatureWriter);
                signatureWriter.Flush();

                var writer = new SshWireWriter(buffer);
                writer.WriteByteString(signatureBuffer.WrittenSpan);
                writer.Flush();
            }
        }

        public delegate void StatusChangedEventHandler(SshAgentPipe pipe, SshAgentPipeStatus status);

        // https://datatracker.ietf.org/doc/html/draft-miller-ssh-agent-04#section-7.1
        internal enum MessageType : byte
        {
            SSH_AGENT_FAILURE = 5,
            SSH_AGENT_SUCCESS = 6,
            SSH_AGENTC_REQUEST_IDENTITIES = 11,
            SSH_AGENT_IDENTITIES_ANSWER = 12,
            SSH_AGENTC_SIGN_REQUEST = 13,
            SSH_AGENT_SIGN_RESPONSE = 14,
            SSH_AGENTC_ADD_IDENTITY = 17,
            SSH_AGENTC_REMOVE_IDENTITY = 18,
            SSH_AGENTC_REMOVE_ALL_IDENTITIES = 19,
            SSH_AGENTC_ADD_SMARTCARD_KEY = 20,
            SSH_AGENTC_REMOVE_SMARTCARD_KEY = 21,
            SSH_AGENTC_LOCK = 22,
            SSH_AGENTC_UNLOCK = 23,
            SSH_AGENTC_ADD_ID_CONSTRAINED = 25,
            SSH_AGENTC_ADD_SMARTCARD_KEY_CONSTRAINED = 26,
            SSH_AGENTC_EXTENSION = 27,
            SSH_AGENT_EXTENSION_FAILURE = 28,
        }

        // https://datatracker.ietf.org/doc/html/draft-miller-ssh-agent-04#section-7.3
        [Flags]
        internal enum SignatureFlags : uint
        {
            None = 0,
            SSH_AGENT_RSA_SHA2_256 = 0x02,
            SSH_AGENT_RSA_SHA2_512 = 0x04,
        }
    }
}
