using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Game;

internal sealed class RouteSeacher : IRoute {
    private Func<Location, GameMap, Boolean> predicate;

    public IEnumerable<MapCellType> Barriers { get; set; }
    public IEnumerable<MapCellType> DefaultBarriers { get; } = new List<MapCellType> { MapCellType.Hidden, MapCellType.Wall };
    private IEnumerable<MapCellType> GetBarriers() => Barriers ?? DefaultBarriers;
    public Location? Destination { get; set; }

    public void AddPredicateOnAvailability(Func<Location, GameMap, Boolean> predicate) => this.predicate += predicate;
    public void RemovePredicate(Func<Location, GameMap, Boolean> predicate) => this.predicate -= predicate;

    public IEnumerable<Location> GetDestinationNodesFormingRoutesWith(Location source, GameMap gameMap) {
        var topLocation = new Location(source.X, source.Y + 1);
        if(IsAvailable(topLocation, gameMap))
            yield return topLocation;
        var bottomLocation = new Location(source.X, source.Y - 1);
        if(IsAvailable(bottomLocation, gameMap))
            yield return bottomLocation;
        var leftLocation = new Location(source.X - 1, source.Y);
        if(IsAvailable(leftLocation, gameMap))
            yield return leftLocation;
        var rightLocation = new Location(source.X + 1, source.Y);
        if(IsAvailable(rightLocation, gameMap))
            yield return rightLocation;
    }

    private Boolean IsAvailable(Location location, GameMap gameMap) {
        if(!gameMap.Contains(location))
            return false;
        if(!IsAvailableByPredicate(location, gameMap))
            return false;
        if(Destination != null && Destination.Value == location)
            return true;
        if(gameMap[location].OneFrom(GetBarriers().ToArray()))
            return false;
        return true;
    }

    private Boolean IsAvailableByPredicate(Location location, GameMap gameMap) {
        if(predicate == null)
            return true;
        return predicate.GetInvocationList().All(p => p == null || ((Func<Location, GameMap, Boolean>)p)(location, gameMap));
    }
}