using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views {
    public struct FieldView : IView {
        public FieldView(Field field, Location? center) {
            this.field = field;
            this.center = center;
        }

        public CellType this[Location index] {
            get {
                if(field == null)
                    return CellType.Empty;
                if(!center.HasValue)
                    return field[index];
                var centerValue = center.Value;
                var offset = (index - centerValue).Abs();
                if(offset.XOffset > field.VisibilityWidth || offset.YOffset > field.VisibilityHeight)
                    return CellType.Hidden;

                return field[index];
            }
        }


        public Int32 Width => field?.Width ?? 0;

        public Int32 Height => field?.Height ?? 0;

        public Int32 VisibilityHeight => field?.VisibilityHeight ?? 0;

        public Int32 VisibilityWidth => field?.VisibilityWidth ?? 0;

        public Boolean Contains(Location location) {
            return field?.Contains(location) ?? false;
        }

        public IEnumerable<Location> GetCellsOfType(CellType type) {
            for(Int32 i = 0; i < Width; i++)
                for(Int32 j = 0; j < Height; j++) {
                    var location = new Location(i, j);
                    if(this[location] == type)
                        yield return location;
                }
        }

        public Boolean HasValue => field != null;

        private readonly Field field;
        private Location? center;
    }
}