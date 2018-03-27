using System;
using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SmallRect {
        public SmallRect(Int16 left, Int16 top, Int16 right, Int16 bottom) {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public readonly Int16 Left;
        public readonly Int16 Top;
        public readonly Int16 Right;
        public readonly Int16 Bottom;
    }
}