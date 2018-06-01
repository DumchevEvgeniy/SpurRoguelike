using System;
using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Extensions {
    internal static class GameMapExtensions {
        public static IEnumerable<Tuple<Location, MapCellType>> GetMapElementsAround(this GameMap gameMap, Location center, Int32 radius = 1) {
            var leftTopCorner = new Location { X = center.X - radius, Y = center.Y - radius };
            var rightBottomCorner = new Location { X = center.X + radius, Y = center.Y + radius };
            foreach(var info in gameMap.GetElementsByRectangle(leftTopCorner, rightBottomCorner))
                if(info.Item1 != center)
                    yield return info;
        }
    }
}