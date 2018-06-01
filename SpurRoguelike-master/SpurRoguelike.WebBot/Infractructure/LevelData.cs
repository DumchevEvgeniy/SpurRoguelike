using System.Collections.Generic;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.WebBot.Infractructure {
    public class LevelData {
        public Field Field { get; set; }
        public Player Player { get; set; }
        public IEnumerable<Monster> Monsters { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public IEnumerable<HealthPack> HealthPacks { get; set; }
    }
}
