using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.Core {
    public class Field {
        public Int32 VisibilityWidth { get; private set; }
        public Int32 VisibilityHeight { get; private set; }

        public Field(Int32 width, Int32 height, Int32 visibilityWidth, Int32 visibilityHeight) {
            VisibilityWidth = visibilityWidth;
            VisibilityHeight = visibilityHeight;
            cells = new CellType[width, height];
        }

        public FieldView CreateView(Location? center) {
            return new FieldView(this, center);
        }

        public CellType this[Location index] {
            get { return cells[index.X, index.Y]; }
            set {
                cells[index.X, index.Y] = value;
                if(value == CellType.PlayerStart)
                    PlayerStart = index;
            }
        }

        public Boolean Contains(Location location) {
            return location.X >= 0 && location.X < Width && location.Y >= 0 && location.Y < Height;
        }

        public IEnumerable<Location> GetCellsOfType(CellType type) {
            for(Int32 i = 0; i < Width; i++)
                for(Int32 j = 0; j < Height; j++)
                    if(cells[i, j] == type)
                        yield return new Location(i, j);
        }

        public Int32 Width => cells.GetLength(0);

        public Int32 Height => cells.GetLength(1);

        public Location PlayerStart { get; private set; }

        private readonly CellType[,] cells;
    }
}