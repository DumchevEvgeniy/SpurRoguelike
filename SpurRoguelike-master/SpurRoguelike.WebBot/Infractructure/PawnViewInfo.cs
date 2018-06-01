using System;

namespace SpurRoguelike.WebPlayerBot.Infractructure {
    public class PawnViewInfo {
        public String Name { get; set; }
        public Int32 Attack { get; set; }
        public Int32 TotalAttack { get; set; }
        public Int32 TotalDefence { get; set; }
        public Int32 Health { get; set; }
        public Guid UniqueId { get; set; }
        public Int32 IsDestroyed { get; set; }
        public Location Location { get; set; }
        public Boolean hasValue { get; set; }
    }
}
