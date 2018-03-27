using System;
using System.Collections;
using System.Collections.Generic;
using SpurRoguelike.PlayerBot.Game;

namespace SpurRoguelike.PlayerBot.Targets {
    internal abstract class BaseTarget : IEnumerator<TurnInfo> {
        protected readonly GameMap gameMap;
        protected TurnInfo current;

        public TurnInfo Current => current;
        Object IEnumerator.Current => Current;

        protected BaseTarget(GameMap gameMap) {
            this.gameMap = gameMap;
        }

        public virtual void Reset() {
            current = null;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(Boolean isDisposing) { }

        public abstract Boolean MoveNext();

        public abstract Boolean IsAvailable();
    }
}