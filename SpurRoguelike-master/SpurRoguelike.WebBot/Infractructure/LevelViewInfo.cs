using System;
using System.Collections.Generic;

namespace SpurRoguelike.WebPlayerBot.Infractructure {
    public class LevelViewInfo {
        public LevelDataViewInfo LevelData { get; set; }
        public Location NorthWestCorner { get; set; }
        public IEnumerable<String> Render { get; set; }
    }
}
