using System.Collections.Generic;

namespace SpurRoguelike.WebPlayerBot.Infractructure {
    public class LevelDataViewInfo {
        public FieldViewInfo Field { get; set; }
        public PawnViewInfo Player { get; set; }
        public IEnumerable<PawnViewInfo> Monsters { get; set; }
        public IEnumerable<ItemViewInfo> Items { get; set; }
        public IEnumerable<HealthPackViewInfo> HealthPacks { get; set; }
    }
}
