# SK SSH Agent

An SSH agent that supports using FIDO/U2F security keys with PuTTY and OpenSSH for Windows.

## Features

 * Pageant IPC — Use security keys with PuTTY and WinSCP.
 * OpenSSH IPC — Use security keys with [OpenSSH for Windows](https://docs.microsoft.com/en-us/windows-server/administration/openssh/openssh_overview).

## Requirements

You need a FIDO/U2F security key, obviously.

### Client

 * Windows 10 (version 1903 or newer)
 * [.NET 6.0 Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0/runtime)
 * SSH Client
   * PuTTY (version 0.75 or newer)
   * WinSCP (version 5.20 or newer)
   * OpenSSH for Windows (version 8.6 or newer)

Note:  Only one agent can be listening on an IPC pipe at a time.  Therefore, Pageant and/or OpenSSH agent cannot be used while SK SSH Agent is running, and vice versa.

### Server

 * OpenSSH 8.2 or newer \
   The `verify-required` option requires OpenSSH 8.3 or newer.

## Getting Started

### Key Generation

 1. Ensure Pageant and/or OpenSSH agent are not running.
 2. Run **SK SSH Agent**.
 3. In the **Key** menu, click **Generate in Security Key...**.
 4. Check the **Require User Verification** checkbox if your security key supports user verification and you want to be prompted for your PIN when you authenticate using your security key.
 5. Click **Generate**.
 6. Windows will guide you through setting up your security key.
 7. When Windows is done setting up your security key, SK SSH Agent will ask where you want to save the private key file.  If you have no preference, use the default location.
 8. SK SSH Agent will ask whether it should load the key.  Click **Yes**.

### Key Authorization

 1. In the **SK SSH Agent** window, select the key in the key list.
 2. In the **Edit** menu, click **Copy OpenSSH Key Authorization**.
 3. Paste the copied key authorization line into `~/.ssh/authorized_keys` on the server.

Authentication works as it normally would with Pageant or OpenSSH agent.

## Limitations

 * The `no-touch-required` option is not supported because the [Windows WebAuthn APIs](https://learn.microsoft.com/en-us/windows/security/identity-protection/hello-for-business/webauthn-apis) do not support it.
 * The `resident` option is not supported.
 * Only the following key types are supported:
   * `ecdsa-sha2-nistp256`
   * `ecdsa-sha2-nistp384`
   * `ecdsa-sha2-nistp521`
   * `ssh-ed25519`
   * `sk-ecdsa-sha2-nistp256@openssh.com`

## Differences from OpenSSH

### Credential Protection

SK SSH Agent does not require the security key to support the Credential Protection (`credProtect`) FIDO extension in order to generate a key with the `verify-required` option.  The authenticator attests (and the SSH server can verify) that user verification was performed, without this extension.

SK SSH Agent will include the `verify-required` option in the key authorization when applicable.  The option will not be present in the public key, so if you use `ssh-copy-id` to authorize the key then you must add the `verify-required` option to the `authorized_keys` file manually.

Some services, such as GitHub, do not support key authorization options.  Without the `verify-required` option, the key will still work, but the user verification will provide no additional security because the service will not verify that user verification was performed.  In this case, Credential Protection is useful.

## Tips

If you supply the path to a key file as a command-line argument when launching SK SSH Agent, it will load the key and start out minimized to the notification area.
