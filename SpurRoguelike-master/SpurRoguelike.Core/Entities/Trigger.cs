using System;

namespace SpurRoguelike.Core.Entities {
    public abstract class Trigger : Entity {
        protected Trigger(String name, Int32 cooldown)
            : base(name) {
            this.cooldown = cooldown;
        }

        public override void Tick() {
            base.Tick();

            if(ticksToCool > 0) {
                ticksToCool--;
                return;
            }

            if(CheckCondition()) {
                ticksToCool = cooldown;
                Fire();
            }
        }

        protected abstract Boolean CheckCondition();

        protected abstract void Fire();

        private readonly Int32 cooldown;

        private Int32 ticksToCool;
    }
}