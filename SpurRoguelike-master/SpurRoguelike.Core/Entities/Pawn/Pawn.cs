using System;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core.Entities {
    public abstract class Pawn : Entity {
        protected Pawn(String name, Int32 attack, Int32 defence, Int32 health, Int32 healthMaximum)
            : base(name) {
            Attack = attack;
            Defence = defence;
            Health = health;
            HealthMaximum = healthMaximum;
        }

        public Int32 Attack { get; private set; }

        public Int32 Defence { get; private set; }

        public Int32 Health { get; private set; }

        public Item EquippedItem { get; private set; }

        public virtual Int32 TotalAttack => Attack + (EquippedItem?.AttackBonus ?? 0);

        public virtual Int32 TotalDefence => Defence + (EquippedItem?.DefenceBonus ?? 0);

        public virtual void PerformAttack(Pawn victim) {
            Level.Player?.EventReporter?.ReportAttack(this, victim);
        }

        public virtual void TakeDamage(Int32 damage, Entity instigator) {
            Level.Player?.EventReporter?.ReportDamage(this, damage, instigator);

            Health = Math.Max(0, Math.Min(Health - damage, HealthMaximum));

            if(Health == 0)
                Destroy();
        }

        public void Upgrade(Int32 attackBonus, Int32 defenceBonus) {
            Attack += attackBonus;
            Defence += defenceBonus;

            Level.Player?.EventReporter?.ReportUpgrade(this, attackBonus, defenceBonus);
        }

        public Boolean IsInAttackRange(Pawn other) {
            return IsInRange(other, 1);
        }

        protected override Boolean ProcessMove(Location newLocation, Level newLevel) {
            if(!base.ProcessMove(newLocation, newLevel))
                return false;

            var destination = newLevel.Field[newLocation];

            if(destination == CellType.Wall)
                return false;

            if(destination == CellType.Trap) {
                ProcessTrap(newLocation);
                return false;
            }

            var pickup = newLevel.GetEntity<Pickup>(newLocation);
            if(pickup != null) {
                ProcessPickup(pickup);
                return false;
            }

            if(newLevel.GetEntity<Monster>(newLocation) != null)
                return false;

            return true;
        }

        public PawnView CreateView() {
            return new PawnView(this);
        }

        protected virtual void ProcessTrap(Location trapLocation) {
            Level.Field[trapLocation] = CellType.Empty;

            Level.Player?.EventReporter?.ReportTrap(this);

            TakeDamage(50, null);
        }

        protected virtual void ProcessPickup(Pickup pickup) {
            if(!pickup.PickUp(this))
                return;

            Level.Player?.EventReporter?.ReportPickup(this, pickup);
        }

        public override void Destroy() {
            base.Destroy();

            EquippedItem?.Drop();

            Level.Player?.EventReporter?.ReportDestroyed(this);
        }

        public void Equip(Item item) {
            EquippedItem?.Drop();
            EquippedItem = item;
        }

        protected readonly Int32 HealthMaximum;
    }
}