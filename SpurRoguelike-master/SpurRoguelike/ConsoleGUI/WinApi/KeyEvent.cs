using System;
using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct KeyEvent {
        public readonly Int32 IsKeyDown;
        public readonly Int16 RepeatCount;
        public readonly Int16 VirtualKeyCode;
        public readonly Int16 VirtualScanCode;

        [MarshalAs(UnmanagedType.U2)]
        public readonly Char UnicodeChar;

        public readonly ControlKeyState ControlKeyState;
    }
}