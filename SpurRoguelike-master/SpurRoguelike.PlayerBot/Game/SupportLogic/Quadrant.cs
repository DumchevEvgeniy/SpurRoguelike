using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.PlayerBot.Game {
    internal struct Quadrant {
        public Location LeftTopCorner { get; set; }
        public Location RightBottomCorner { get; set; }
        public Location RightTopCorner => new Location(RightBottomCorner.X, LeftTopCorner.Y);
        public Location LeftBottomCorner => new Location(LeftTopCorner.X, RightBottomCorner.Y);

        public IEnumerable<Location> GetAllCorners() {
            yield return LeftTopCorner;
            yield return RightTopCorner;
            yield return LeftBottomCorner;
            yield return RightBottomCorner;
        }
    }
}