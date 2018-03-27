using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Game;

namespace SpurRoguelike.PlayerBot.Extensions {
    internal static class GameMapExtensions {
        public static IEnumerable<Tuple<Location, MapCellType>> GetMapElementsAround(this GameMap gameMap, Location center, Int32 radius = 1) {
            var leftTopCorner = new Location(center.X - radius, center.Y - radius);
            var rightBottomCorner = new Location(center.X + radius, center.Y + radius);
            foreach(var info in gameMap.GetElementsByRectangle(leftTopCorner, rightBottomCorner))
                if(info.Item1 != center)
                    yield return info;
        }
    }
}