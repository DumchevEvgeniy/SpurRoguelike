using System;
using System.Collections.Generic;

namespace SpurRoguelike.WebBot.Infractructure {
    public class LevelViewInfo {
        public LevelDataViewInfo LevelData { get; set; }
        public LocationViewInfo NorthWestCorner { get; set; }
        public IEnumerable<String> Render { get; set; }
    }
}
