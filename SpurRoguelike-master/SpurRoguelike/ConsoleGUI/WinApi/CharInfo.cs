using System;
using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct CharInfo {
        public CharInfo(Char unicodeChar, CharAttributes attributes) {
            UnicodeChar = unicodeChar;
            Attributes = attributes;
        }

        [MarshalAs(UnmanagedType.U2)]
        public readonly Char UnicodeChar;
        public readonly CharAttributes Attributes;
    }
}