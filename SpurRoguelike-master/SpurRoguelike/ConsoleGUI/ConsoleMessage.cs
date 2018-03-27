using System;

namespace SpurRoguelike.ConsoleGUI {
    internal struct ConsoleMessage {
        public ConsoleMessage(String text, ConsoleColor color, ConsoleColor backgroundColor = ConsoleColor.Black) {
            Text = text;
            Color = color;
            BackgroundColor = backgroundColor;
        }

        public readonly String Text;
        public readonly ConsoleColor Color;
        public readonly ConsoleColor BackgroundColor;
    }
}