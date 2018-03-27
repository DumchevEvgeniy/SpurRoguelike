using System;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Entities {
    public abstract class Entity {
        protected Entity(String name) {
            Name = name;
        }

        public String Name { get; }

        public Location Location { get; protected set; }

        public Level Level { get; protected set; }

        public Boolean IsDestroyed { get; private set; } = false;

        public virtual void Tick() {
        }

        public Boolean Move(Location newLocation, Level newLevel) {
            var oldLocation = Location;

            if(!newLevel.Field.Contains(newLocation) || !ProcessMove(newLocation, newLevel))
                return false;

            Location = newLocation;

            Level?.ProcessMove(this, oldLocation, Location);

            if(Level != newLevel) {
                Level?.Destroy(this);
                Level = newLevel;
                newLevel.Spawn(newLocation, this);
            }

            return true;
        }

        public Boolean IsInRange(Entity other, Int32 range) {
            return Location.IsInRange(other.Location, range) && !other.IsDestroyed && Level == other.Level;
        }

        protected virtual Boolean ProcessMove(Location newLocation, Level newLevel) {
            return true;
        }

        public virtual void Destroy() {
            IsDestroyed = true;
            Level.Destroy(this);
        }
    }
}