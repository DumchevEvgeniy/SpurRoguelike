using System;
using SpurRoguelike.Core;

namespace SpurRoguelike.ConsoleGUI {
    internal class ConsoleRenderer : IRenderer {
        public ConsoleRenderer(ConsoleGui gui) {
            this.gui = gui;
        }

        public void RenderLevel(Level level) {
            gui.RenderLevel(level);
        }

        public void RenderGameEnd(Boolean isCompleted) {
            gui.EndGame(isCompleted);
        }

        private readonly ConsoleGui gui;
    }
}