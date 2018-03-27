using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Game;

internal sealed class RouteSeacherBuilder {
    public List<Location> FoundHiddenLocation { get; } = new List<Location>();

    private readonly RouteSeacher routeSeacher = new RouteSeacher(); 

    public RouteSeacher Create() {
        FoundHiddenLocation.Clear();
        return routeSeacher;
    }

    public RouteSeacherBuilder AddHiddenCellSaver() {
        routeSeacher.AddPredicateOnAvailability((location, gameMap) => {
            if(gameMap[location] == MapCellType.Hidden)
                FoundHiddenLocation.Add(location);
            return true;
        });
        return this;
    }

    public RouteSeacherBuilder AddPredicateOnAvailability(Func<Location, GameMap, Boolean> predicate) {
        routeSeacher.AddPredicateOnAvailability(predicate);
        return this;
    }

    public RouteSeacherBuilder AddBarriers(params MapCellType[] barriers) {
        routeSeacher.Barriers = barriers;
        return this;
    }

    public RouteSeacherBuilder AddDestination(Location destination) {
        routeSeacher.Destination = destination;
        return this;
    }
}
