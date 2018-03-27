using System;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core.Entities {
    public class HealthPack : Pickup {
        public HealthPack(String name)
            : base(name) {
        }

        public override Boolean PickUp(Pawn pawn) {
            pawn.TakeDamage(-50, null);
            Destroy();
            return true;
        }

        public HealthPackView CreateView() {
            return new HealthPackView(this);
        }
    }
}