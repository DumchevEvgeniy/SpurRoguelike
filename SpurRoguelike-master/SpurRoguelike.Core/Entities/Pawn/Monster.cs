using System;

namespace SpurRoguelike.Core.Entities {
    public abstract class Monster : Pawn {
        protected Monster(String name, Int32 attack, Int32 defence, Int32 health, Int32 healthMaximum)
            : base(name, attack, defence, health, healthMaximum) {
        }

        public override void PerformAttack(Pawn victim) {
            if(victim is Monster)
                return;

            base.PerformAttack(victim);

            var damage = (Int32)(((Double)TotalAttack / victim.TotalDefence) * BaseDamage * (1 - Level.Random.NextDouble() / 10));
            victim.TakeDamage(damage, this);
        }

        private const Int32 BaseDamage = 10;
    }
}