using System;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core {
    internal class EntitiesIndex {
        public EntitiesIndex(Int32 width, Int32 height) {
            cells = new Entity[width, height];
        }

        public Entity this[Location index] {
            get { return cells[index.X, index.Y]; }
            set {
                cells[index.X, index.Y] = value;
            }
        }

        public Boolean Contains(Location location) {
            return location.X >= 0 && location.X < Width && location.Y >= 0 && location.Y < Height;
        }

        public void Move(Entity entity, Location from, Location to) {
            if(Contains(to))
                this[to] = entity;
            if(Contains(from) && this[from] == entity)
                this[from] = null;
        }

        public Int32 Width => cells.GetLength(0);

        public Int32 Height => cells.GetLength(1);

        private readonly Entity[,] cells;
    }
}