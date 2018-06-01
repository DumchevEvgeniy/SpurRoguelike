using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

internal interface IRoute {
    IEnumerable<Location> GetDestinationNodesFormingRoutesWith(Location source, GameMap gameMap);
}