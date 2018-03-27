using System;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Content {
    internal class Dimwit : Monster {
        public Dimwit(String name, Int32 attack, Int32 defence, Int32 health, Int32 healthMaximum)
            : base(name, attack, defence, health, healthMaximum) {
        }

        public override void Tick() {
            base.Tick();

            if(!IsInRange(Level.Player, SeeRadius))
                return;

            if(IsInAttackRange(Level.Player)) {
                PerformAttack(Level.Player);
                return;
            }

            var stepDirection = Level.Random.Select(Offset.StepOffsets);

            Move(Location + stepDirection, Level);
        }

        private const Int32 SeeRadius = 15;
    }
}