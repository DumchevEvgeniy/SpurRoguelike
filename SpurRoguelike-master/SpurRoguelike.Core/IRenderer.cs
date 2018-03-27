using System;

namespace SpurRoguelike.Core {
    public interface IRenderer {
        void RenderLevel(Level level);

        void RenderGameEnd(Boolean isCompleted);
    }
}