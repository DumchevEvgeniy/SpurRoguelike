using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators {
    public class MonsterClassOptions {
        public Double Skill { get; set; }
        public Func<String, Double, Int32, Int32, Int32, Monster> Factory { get; set; }
        public Double Rarity { get; set; }
    }
}