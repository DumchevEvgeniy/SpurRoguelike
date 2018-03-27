using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Game;

namespace SpurRoguelike.PlayerBot.Targets {
    internal abstract class BaseTargetMovementOnBestPositionWhenMonsters : BaseTargetWithSavesOrRecalculateRoute {
        protected BaseTargetMovementOnBestPositionWhenMonsters(GameMap gameMap) : base(gameMap) { }

        protected override TargetRouteInfo TryGetRoute() {
            IEnumerable<Location> bestRoute = null;
            Int32 maxSumDamage = Int32.MaxValue;
            foreach(var targetLocation in GetTargetLocations()) {
                var route = CreateMovement(targetLocation, maxSumDamage).GetRoute(gameMap.AreaInfo.Player.Location, targetLocation);
                if(route == null)
                    continue;
                var currentSumDamage = new DamageOnRouteCalculator(gameMap).GetMaxSumDamageOnRoute(route, maxSumDamage);
                if(currentSumDamage <= gameMap.AreaInfo.Player.Health)
                    return new TargetRouteInfo(route, true);
                if(bestRoute == null || currentSumDamage < maxSumDamage) {
                    maxSumDamage = currentSumDamage;
                    bestRoute = route;
                }
            }
            if(bestRoute == null || !BestRouteIsAvailable(bestRoute, maxSumDamage))
                return null;
            return new TargetRouteInfo(bestRoute, false);
        }

        private IMovement CreateMovement(Location destination, Int32 minWeightLimit) {
            var routeSeacher = new RouteSeacherBuilder().AddDestination(destination).AddBarriers(Barriers.ToArray()).Create();
            var movement = new Movement<Int32>(gameMap, routeSeacher, new WeightCalculator(minWeightLimit, AdditionalWeightCalculator));
            movement.AddPredicateOnAvailability((currentSourceNode, map) => currentSourceNode.Weight != Int32.MaxValue);
            return movement;
        }

        protected virtual Boolean BestRouteIsAvailable(IEnumerable<Location> bestRoute, Int32 maxSumDamage) => true;

        protected abstract IEnumerable<MapCellType> Barriers { get; }
        protected virtual IWeightCalculator<Int32> AdditionalWeightCalculator { get; }
        protected abstract IEnumerable<Location> GetTargetLocations();

        private class WeightCalculator : IWeightCalculator<Int32> {
            private readonly Int32 minSumDamage;
            private readonly IWeightCalculator<Int32> additionalWeightCalculator;

            public WeightCalculator(Int32 minSumDamage, IWeightCalculator<Int32> additionalWeightCalculator) {
                this.minSumDamage = minSumDamage;
                this.additionalWeightCalculator = additionalWeightCalculator;
            }

            public Int32 GetWeight(PonderableNode<Int32> source, PonderableNode<Int32> next, PonderableNode<Int32> destination, GameMap map) {
                var weightByPossibleDamage = new DamageOnRouteCalculator(map).GetMaxSumDamageOnRoute(next.GetAscendantLocations(), minSumDamage);
                if(weightByPossibleDamage == Int32.MaxValue)
                    return Int32.MaxValue;
                var additionalWeight = additionalWeightCalculator != null ? additionalWeightCalculator.GetWeight(source, next, destination, map) : 0;
                if(additionalWeight == Int32.MaxValue)
                    return Int32.MaxValue;
                return weightByPossibleDamage + additionalWeight;
            }
        }
    }
}
