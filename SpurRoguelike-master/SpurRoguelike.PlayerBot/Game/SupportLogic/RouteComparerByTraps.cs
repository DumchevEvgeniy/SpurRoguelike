using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.PlayerBot.Game {
    internal class RouteComparerByTraps : IComparer<IEnumerable<Location>> {
        private GameMap gameMap;

        public RouteComparerByTraps(GameMap gameMap) {
            this.gameMap = gameMap;
        }

        public Int32 Compare(IEnumerable<Location> route1, IEnumerable<Location> route2) {
            if(route1 == null && route2 == null)
                return 0;
            if(route1 == null)
                return 1;
            if(route2 == null)
                return -1;
            return GetNumberOfTraps(route1).CompareTo(GetNumberOfTraps(route2));
        }

        private Int32 GetNumberOfTraps(IEnumerable<Location> locations) => locations.Count(loc => gameMap[loc] == MapCellType.Trap);
    }

}