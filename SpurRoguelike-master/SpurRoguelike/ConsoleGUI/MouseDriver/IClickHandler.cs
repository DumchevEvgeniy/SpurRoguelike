using System;

namespace SpurRoguelike.ConsoleGUI.MouseDriver {
    internal interface IClickHandler {
        void HandleMouseClick(Int32 mouseLeft, Int32 mouseTop);
    }
}