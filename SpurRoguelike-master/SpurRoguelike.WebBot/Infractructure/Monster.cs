using System;

namespace SpurRoguelike.WebBot.Infractructure {
    public class Monster {
        public String Name { get; set; }
        public Int32 attack { get; set; }
        public Int32 defence { get; set; }
        public Int32 totalAttack { get; set; }
        public Int32 totalDefence { get; set; }
        public Int32 health { get; set; }
        public Guid uniqueId { get; set; }
        public Boolean isDestroyed { get; set; }
        public Location location { get; set; }
    }
}
