using System;
using System.Runtime.InteropServices;

namespace SpurRoguelike.ConsoleGUI.WinApi {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct Coord {
        public Coord(Int16 x, Int16 y) {
            X = x;
            Y = y;
        }

        public readonly Int16 X;
        public readonly Int16 Y;
    }
}