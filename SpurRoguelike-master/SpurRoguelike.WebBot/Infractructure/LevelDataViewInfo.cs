using System.Collections.Generic;

namespace SpurRoguelike.WebBot.Infractructure {
    public class LevelDataViewInfo {
        public FieldViewInfo Field { get; set; }
        public PlayerViewInfo Player { get; set; }
        public IEnumerable<MonsterViewInfo> Monsters { get; set; }
        public IEnumerable<ItemViewInfo> Items { get; set; }
        public IEnumerable<HealthPackViewInfo> HealthPacks { get; set; }
    }
}
