using System;

namespace SpurRoguelike.ConsoleGUI {
    internal struct ScreenZone {
        public ScreenZone(Int32 left, Int32 top, Int32 width, Int32 height) {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public readonly Int32 Left;
        public readonly Int32 Top;
        public readonly Int32 Width;
        public readonly Int32 Height;

    }
}