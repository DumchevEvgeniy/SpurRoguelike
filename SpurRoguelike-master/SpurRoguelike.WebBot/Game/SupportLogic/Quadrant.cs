using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal struct Quadrant {
        public Location LeftTopCorner { get; set; }
        public Location RightBottomCorner { get; set; }
        public Location RightTopCorner => new Location { X = RightBottomCorner.X, Y = LeftTopCorner.Y };
        public Location LeftBottomCorner => new Location { X = LeftTopCorner.X, Y = RightBottomCorner.Y };

        public IEnumerable<Location> GetAllCorners() {
            yield return LeftTopCorner;
            yield return RightTopCorner;
            yield return LeftBottomCorner;
            yield return RightBottomCorner;
        }
    }
}