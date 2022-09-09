// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using static Windows.Win32.PInvoke;

namespace SKSshAgent
{
    internal class PasswordTextBox : TextBox
    {
        private const int _arrayMaxLength = 0x7FEFFFFF;

        private char[] _buffer = Array.Empty<char>();

        private int _length;

        private Keys _modifierKeys;

        private bool _allowDeleteKeyUp;

        private string _dummyText = string.Empty;

        public PasswordTextBox()
        {
            UseSystemPasswordChar = true;
        }

        /// <exception cref="InvalidOperationException"/>
        [Obsolete("Use GetPassword().", error: true)]
        public new string Text
        {
            get => throw new InvalidOperationException();
            set => throw new InvalidOperationException();
        }

        public ShieldedImmutableBuffer GetPassword()
        {
            var password = _buffer.AsSpan(0, _length);
            int length = Encoding.UTF8.GetByteCount(password);
            return ShieldedImmutableBuffer.Create<char>(length, password, (source, buffer) => Encoding.UTF8.GetBytes(source, buffer));
        }

        public void ZeroMemory()
        {
            CryptographicOperations.ZeroMemory(MemoryMarshal.AsBytes<char>(_buffer));

            _buffer = Array.Empty<char>();
            _length = 0;

            UpdateText(0);
        }

        protected override void Dispose(bool disposing)
        {
            ZeroMemory();

            base.Dispose(disposing);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (base.Text != _dummyText)
            {
                Debug.Assert(false);

                base.Text = _dummyText;

                SystemSounds.Beep.Play();
                return;
            }

            base.OnTextChanged(e);
        }

        protected override bool ProcessKeyMessage(ref Message m)
        {
            switch ((uint)m.Msg)
            {
                case WM_CHAR:
                    return WmChar(m);

                case WM_KEYDOWN:
                    return WmKeyDown(m);

                case WM_KEYUP:
                    return WmKeyUp(m);
            }

            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch ((uint)m.Msg)
            {
                case EM_REPLACESEL:
                    EmReplaceSel(ref m);
                    return;

                case WM_PASTE:
                    WmPaste(ref m);
                    return;

                case WM_CLEAR:
                    WmClear(ref m);
                    return;
            }

            base.WndProc(ref m);
        }

        // Invoked using Control+Backspace.
        private void EmReplaceSel(ref Message m)
        {
            string replacement = Marshal.PtrToStringAuto(m.LParam)!;

            int selectionStart = SelectionStart;
            ReplaceRange(selectionStart, SelectionLength, replacement);

            UpdateText(selectionStart + replacement.Length);
        }

        private bool WmChar(Message m)
        {
            char keyChar = (char)m.WParam;

            if (keyChar < ' ')
            {
                switch (keyChar)
                {
                    case '\u0001':  // Control+A is select all.
                    case '\u0003':  // Control+C is copy.
                    case '\u0016':  // Control+V is paste.
                    case '\u0018':  // Control+X is cut.
                    case '\u001A':  // Control+Z is undo.
                        return false;

                    case '\u0008':  // Control+H is backspace.
                        return true;

                    case '\u0009':  // Control+I is tab.
                    case '\u000A':  // Control+J is line feed.
                    case '\u000D':  // Control+M is carriage return.
                    default:        // And the rest of the control codes, which aren't appropriate for text box input.
                        SystemSounds.Beep.Play();
                        return true;
                }
            }
            else
            {
                int selectionStart = SelectionStart;
                ReplaceRange(selectionStart, SelectionLength, keyChar);

                UpdateText(selectionStart + 1);

                return true;
            }
        }

        private bool WmKeyDown(Message m)
        {
            var keyCode = (VIRTUAL_KEY)m.WParam;
            Keys modifierKeys = _modifierKeys;

            // Control+H is backspace.
            if ((modifierKeys & Keys.Control) != 0 &&
                keyCode == VIRTUAL_KEY.VK_H)
            {
                keyCode = VIRTUAL_KEY.VK_BACK;
                modifierKeys &= ~Keys.Control;
            }

            switch (keyCode)
            {
                case VIRTUAL_KEY.VK_SHIFT:
                    _modifierKeys |= Keys.Shift;
                    return false;
                case VIRTUAL_KEY.VK_CONTROL:
                    _modifierKeys |= Keys.Control;
                    return false;
                case VIRTUAL_KEY.VK_END:
                case VIRTUAL_KEY.VK_HOME:
                case VIRTUAL_KEY.VK_LEFT:
                case VIRTUAL_KEY.VK_UP:
                case VIRTUAL_KEY.VK_RIGHT:
                case VIRTUAL_KEY.VK_DOWN:
                case VIRTUAL_KEY.VK_INSERT:
                    return false;
                case VIRTUAL_KEY.VK_BACK:
                {
                    // Control(+Shift)+Backspace is handled before reaching here.
                    Debug.Assert((modifierKeys & Keys.Control) == 0);

                    if (SelectionLength > 0)
                    {
                        int selectionStart = SelectionStart;
                        RemoveRange(selectionStart, SelectionLength);

                        UpdateText(selectionStart);
                    }
                    else if (SelectionStart > 0)
                    {
                        if ((modifierKeys & Keys.Control) != 0)
                        {
                            RemoveRange(0, SelectionStart);

                            UpdateText(0);
                        }
                        else
                        {
                            int selectionStart = SelectionStart - 1;
                            RemoveRange(selectionStart, 1);

                            UpdateText(selectionStart);
                        }
                    }
                    return true;
                }
                case VIRTUAL_KEY.VK_DELETE:
                {
                    if (SelectionLength > 0)
                    {
                        // Shift+Delete is cut when there is a selection.
                        if (modifierKeys == Keys.Shift)
                        {
                            _allowDeleteKeyUp = true;
                            return false;
                        }

                        int selectionStart = SelectionStart;
                        RemoveRange(selectionStart, SelectionLength);

                        UpdateText(selectionStart);
                    }
                    else if (SelectionStart < TextLength)
                    {
                        if ((modifierKeys & Keys.Control) != 0)
                        {
                            int selectionStart = SelectionStart;
                            RemoveRange(selectionStart, TextLength - SelectionStart);

                            UpdateText(selectionStart);
                        }
                        else
                        {
                            int selectionStart = SelectionStart;
                            RemoveRange(selectionStart, 1);

                            UpdateText(selectionStart);
                        }
                    }
                    return true;
                }
                default:
                    return true;
            }
        }

        private bool WmKeyUp(Message m)
        {
            var keyCode = (VIRTUAL_KEY)m.WParam;

            switch (keyCode)
            {
                case VIRTUAL_KEY.VK_SHIFT:
                    _modifierKeys &= ~Keys.Shift;
                    return false;
                case VIRTUAL_KEY.VK_CONTROL:
                    _modifierKeys &= ~Keys.Control;
                    return false;
                case VIRTUAL_KEY.VK_END:
                case VIRTUAL_KEY.VK_HOME:
                case VIRTUAL_KEY.VK_LEFT:
                case VIRTUAL_KEY.VK_UP:
                case VIRTUAL_KEY.VK_RIGHT:
                case VIRTUAL_KEY.VK_DOWN:
                case VIRTUAL_KEY.VK_INSERT:
                    return false;
                case VIRTUAL_KEY.VK_BACK:
                    return true;
                case VIRTUAL_KEY.VK_DELETE:
                    if (_allowDeleteKeyUp)
                    {
                        _allowDeleteKeyUp = false;
                        return false;
                    }
                    return true;
                default:
                    return true;
            }
        }

        // Invoked by Control+V, Shift+Insert, or "Paste" in the context menu.
        private void WmPaste(ref Message m)
        {
            _ = m;

            string replacement;
            try
            {
                replacement = Clipboard.GetText();
            }
            catch (ExternalException)
            {
                return;
            }

            int selectionStart = SelectionStart;
            ReplaceRange(selectionStart, SelectionLength, replacement);

            UpdateText(selectionStart + replacement.Length);
        }

        // Would be invoked using "Delete" in the context menu, but it's disabled on password text boxes.
        private void WmClear(ref Message m)
        {
            _ = m;

            int selectionStart = SelectionStart;
            RemoveRange(selectionStart, SelectionLength);

            UpdateText(selectionStart);
        }

        private void UpdateText(int selectionStart)
        {
            _dummyText = new string('\u007F', _length);

            base.Text = _dummyText;
            
            SelectionStart = selectionStart;
        }

        private void EnsureCapacity(int capacity)
        {
            if (capacity < _buffer.Length)
                return;

            uint minCapacity = (uint)capacity;
            uint newCapacity = Math.Max(minCapacity, (uint)_buffer.Length * 2);
            if (newCapacity > _arrayMaxLength)
                newCapacity = Math.Max(minCapacity, _arrayMaxLength);

            char[] newBuffer = GC.AllocateUninitializedArray<char>((int)newCapacity, pinned: true);
            Array.Copy(_buffer, newBuffer, _length);
            Array.Clear(newBuffer, _length, newBuffer.Length - _length);
            CryptographicOperations.ZeroMemory(MemoryMarshal.AsBytes<char>(_buffer));
            _buffer = newBuffer;
        }

        private void ReplaceRange(int start, int length, char c)
        {
            int end = start + length;

            int newLength = _length - length + 1;

            EnsureCapacity(newLength);

            Array.Copy(_buffer, end, _buffer, start + 1, _length - end);
            _buffer[start] = c;
            _length = newLength;
        }

        private void ReplaceRange(int start, int length, string s)
        {
            int end = start + length;

            int newLength = _length - length + s.Length;

            EnsureCapacity(newLength);

            Array.Copy(_buffer, end, _buffer, start + s.Length, _length - end);
            s.CopyTo(0, _buffer, start, s.Length);
            _length = newLength;
        }

        private void RemoveRange(int start, int length)
        {
            int end = start + length;

            Array.Copy(_buffer, end, _buffer, start, _length - end);
            _length -= length;
        }
    }
}
