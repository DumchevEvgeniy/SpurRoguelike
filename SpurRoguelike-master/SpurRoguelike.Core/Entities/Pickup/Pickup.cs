using System;

namespace SpurRoguelike.Core.Entities {
    public abstract class Pickup : Entity {
        protected Pickup(String name)
            : base(name) {
        }

        public abstract Boolean PickUp(Pawn pawn);
    }
}