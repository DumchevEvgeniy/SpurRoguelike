using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators {
    internal class MonsterClass {
        public MonsterClass(Func<Monster> factory, Double rarity, Double skill) {
            Factory = factory;
            Rarity = rarity;
            Skill = skill;
        }

        public Func<Monster> Factory { get; }

        public Double Rarity { get; }

        public Double Skill { get; }
    }
}