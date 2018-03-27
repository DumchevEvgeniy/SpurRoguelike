using System;

namespace SpurRoguelike.ConsoleGUI {
    internal interface ITextScreen {
        void Clear();

        void Put(Int32 left, Int32 top, ConsoleCharacter character);

        void Fill(ScreenZone zone, ConsoleCharacter character);

        void Write(Int32 left, Int32 top, ConsoleMessage message);

        void Write(Int32 left, Int32 top, ConsoleMessage message, Int32 redrawLength);

        void Render();

        void Render(ScreenZone zone);

        Int32 Width { get; }

        Int32 Height { get; }
    }
}