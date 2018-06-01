using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

internal class Movement<TWeight> : IMovement where TWeight : IComparable<TWeight> {
    private MovementEnumerable<TWeight> nodesOnTargetRoute;
    private GameMap gameMap;
    private IRoute routeSeacher;
    private IWeightCalculator<TWeight> weightCalculator;
    private Func<PonderableNode<TWeight>, GameMap, Boolean> predicate;

    public Movement(GameMap gameMap, IRoute routeSeacher, IWeightCalculator<TWeight> weightCalculator) {
        this.gameMap = gameMap;
        this.routeSeacher = routeSeacher;
        this.weightCalculator = weightCalculator;
    }

    public IEnumerable<Location> GetRoute(Location sourceLocation, Location destinationLocation) {
        if(sourceLocation == destinationLocation)
            return null;
        var source = new PonderableNode<TWeight>(sourceLocation, weightCalculator);
        var destination = new PonderableNode<TWeight>(destinationLocation, weightCalculator);
        nodesOnTargetRoute = new MovementEnumerable<TWeight>() { source };
        foreach(var currentSourceNode in nodesOnTargetRoute) {
            if(!IsAvailable(currentSourceNode))
                return null;
            if(currentSourceNode.Equals(destination))
                return currentSourceNode.GetAscendantLocations();
            FindTransitionRoutes(currentSourceNode, destination);
        }
        return null;
    }

    public void AddPredicateOnAvailability(Func<PonderableNode<TWeight>, GameMap, Boolean> predicate) => this.predicate += predicate;
    public void RemovePredicate(Func<PonderableNode<TWeight>, GameMap, Boolean> predicate) => this.predicate -= predicate;

    private Boolean IsAvailable(PonderableNode<TWeight> currentSourceNode) =>
        IsAvailableByPredicate(currentSourceNode, gameMap);

    private Boolean IsAvailableByPredicate(PonderableNode<TWeight> currentSourceNode, GameMap gameMap) {
        if(predicate == null)
            return true;
        return predicate.GetInvocationList().All(p => 
            p == null || ((Func<PonderableNode<TWeight>, GameMap, Boolean>)p)(currentSourceNode, gameMap));
    }

    private void FindTransitionRoutes(PonderableNode<TWeight> currentSourceNode, PonderableNode<TWeight> destination) {
        var possibleNodesFormingRoutes = routeSeacher.GetDestinationNodesFormingRoutesWith(currentSourceNode.Location, gameMap);
        foreach(var locationNodeFormingRoute in possibleNodesFormingRoutes) {
            if(nodesOnTargetRoute.WasVisited(locationNodeFormingRoute))
                continue;
            var node = nodesOnTargetRoute.Find(locationNodeFormingRoute);
            if(node == null)
                nodesOnTargetRoute.Add(CreatePonderableNode(locationNodeFormingRoute, currentSourceNode, destination));
            else
                RecalculateParameters(currentSourceNode, node, destination);
        }
    }

    private void RecalculateParameters(PonderableNode<TWeight> from, PonderableNode<TWeight> to, PonderableNode<TWeight> destination) {
        var weight = from.CalculateWeight(to, destination, gameMap);
        if(to.CompareTo(weight) > 0) {
            to.Weight = weight;
            to.PreviousNode = from;
        }
    }

    private PonderableNode<TWeight> CreatePonderableNode(Location location, PonderableNode<TWeight> previous, PonderableNode<TWeight> destination) {
        var newNode = new PonderableNode<TWeight>(location, weightCalculator);
        if(previous != null) {
            newNode.PreviousNode = previous;
            newNode.Weight = previous.CalculateWeight(newNode, destination, gameMap);
        }
        return newNode;
    }
}
