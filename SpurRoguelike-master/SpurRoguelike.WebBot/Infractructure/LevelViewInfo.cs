using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Game;

namespace SpurRoguelike.WebPlayerBot.Infractructure {
    public class LevelViewInfo {
        public LevelDataViewInfo LevelData { get; set; }
        public Location NorthWestCorner { get; set; }
        public IEnumerable<String> Render { get; set; }
    }

    class ParticalField {
        private readonly List<String> cells;
        private readonly LevelViewInfo levelViewInfo;

        public ParticalField(LevelViewInfo levelViewInfo) {
            cells = levelViewInfo.Render.ToList();
            this.levelViewInfo = levelViewInfo;
        }

        public MapCellType GetValue(Location location) {
            return cells[location]
        }

        public void Go() {
            var countColumns = cells.First().Length;
            for(Int32 indexColumn = 0; indexColumn < cells.First().Length; indexColumn++) {
                for(Int32 indexRow = 0; indexRow < cells.Count; indexRow++) {

                }
            }
        }

    }

}
