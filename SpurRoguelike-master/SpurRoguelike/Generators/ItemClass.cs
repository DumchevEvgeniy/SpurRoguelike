using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators {
    internal class ItemClass {
        public ItemClass(Func<Item> factory, Double rarity, Int32 level) {
            Factory = factory;
            Rarity = rarity;
            Level = level;
        }

        public Func<Item> Factory { get; }

        public Double Rarity { get; }

        public Int32 Level { get; set; }
    }
}