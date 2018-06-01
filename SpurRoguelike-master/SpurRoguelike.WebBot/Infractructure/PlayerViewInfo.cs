using System;

namespace SpurRoguelike.WebBot.Infractructure {
    public class PlayerViewInfo {
        public String Name { get; set; }
        public Int32 Attack { get; set; }
        public Int32 Defence { get; set; }
        public Int32 TotalAttack { get; set; }
        public Int32 TotalDefence { get; set; }
        public Int32 Health { get; set; }
        public Guid UniqueId { get; set; }
        public Boolean IsDestroyed { get; set; }
        public LocationViewInfo Location { get; set; }
    }
}
