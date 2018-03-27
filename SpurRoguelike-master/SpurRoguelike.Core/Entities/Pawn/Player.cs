using System;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Entities {
    public class Player : Pawn {
        public Player(String name, Int32 attack, Int32 defence, Int32 health, Int32 healthMaximum, IPlayerController playerController, IEventReporter eventReporter)
            : base(name, attack, defence, health, healthMaximum) {
            EventReporter = eventReporter;
            this.playerController = playerController;
        }

        public override void Tick() {
            base.Tick();

            playerController.MakeTurn(Level.CreateView(), EventReporter).Apply(this);
        }

        public override void PerformAttack(Pawn victim) {
            base.PerformAttack(victim);

            var damage = (Int32)(((Double)TotalAttack / victim.TotalDefence) * BaseDamage * (1 - Level.Random.NextDouble() / 10));
            victim.TakeDamage(damage, this);

            if(victim is Monster && victim.IsDestroyed) {
                var attackBonus = (Int32)(((Double)victim.Attack / Attack) * BaseUpgrade * (1 + Level.Random.NextDouble() / 4));
                var defenceBonus = (Int32)(((Double)victim.Defence / Defence) * BaseUpgrade * (1 + Level.Random.NextDouble() / 4));

                Upgrade(attackBonus, defenceBonus);
            }
        }

        public IEventReporter EventReporter { get; }

        protected override Boolean ProcessMove(Location newLocation, Level newLevel) {
            if(!base.ProcessMove(newLocation, newLevel))
                return false;

            var destination = newLevel.Field[newLocation];

            if(destination == CellType.Exit) {
                Level.Complete();
                return false;
            }

            return true;
        }

        private readonly IPlayerController playerController;

        private const Int32 BaseDamage = 10;
        private const Double BaseUpgrade = 1.5;
    }
}