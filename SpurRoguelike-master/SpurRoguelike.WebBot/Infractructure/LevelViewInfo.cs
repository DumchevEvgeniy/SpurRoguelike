using System;
using System.Collections.Generic;

namespace SpurRoguelike.WebBot.Infractructure {
    public class LevelViewInfo {
        public LevelData LevelData { get; set; }
        public Location NorthWestCorner { get; set; }
        public IEnumerable<String> Render { get; set; }
    }
}
