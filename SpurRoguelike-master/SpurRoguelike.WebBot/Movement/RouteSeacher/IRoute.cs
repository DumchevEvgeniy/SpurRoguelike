using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Game;

internal interface IRoute {
    IEnumerable<Location> GetDestinationNodesFormingRoutesWith(Location source, GameMap gameMap);
}