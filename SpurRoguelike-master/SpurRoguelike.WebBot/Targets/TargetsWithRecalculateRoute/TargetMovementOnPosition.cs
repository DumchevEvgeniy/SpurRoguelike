using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal sealed class TargetMovementOnPosition : BaseTargetWithRecalculateRoute {
        public TargetMovementOnPosition(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() => true;

        protected override IEnumerable<Location> TryGetRoute() {
            var routes = new List<Location>();
            var locations = gameMap.GetMapElementsAround(gameMap.AreaInfo.Player.Location, 4)
                .Where(info => info.Item2.OneFrom(MapCellType.HealthPack, MapCellType.Item, MapCellType.None))
                .Select(info => info.Item1)
                .OrderBy(loc => (loc - gameMap.AreaInfo.Player.Location).Size());
            foreach(var location in locations) {
                var route = CreateMovement(location).GetRoute(gameMap.AreaInfo.Player.Location, location);
                if(route != null)
                    routes.Add(route.Skip(1).First());
            }
            if(routes.Count == 0)
                return null;
            var dictionaryLocations = new Dictionary<Location, Int32>();
            foreach(var location in routes) {
                if(!dictionaryLocations.ContainsKey(location))
                    dictionaryLocations.Add(location, 1);
                else
                    dictionaryLocations[location]++;
            }
            return new List<Location>{
                gameMap.AreaInfo.Player.Location,
                dictionaryLocations.Aggregate((info1, info2) => info1.Value >= info2.Value ? info1 : info2).Key
            };
        }

        private IMovement CreateMovement(Location destination) {
            var routeSeacher = new RouteSeacher {
                Destination = destination,
                Barriers = new List<MapCellType> { MapCellType.Exit, MapCellType.Hidden, MapCellType.Monster, MapCellType.Trap, MapCellType.Wall }
            };
            return new Movement<Int32>(gameMap, routeSeacher, new WeightCalculatorForSafeMovement());
        }

        private class WeightCalculatorForSafeMovement : IWeightCalculator<Int32> {
            public Int32 GetWeight(PonderableNode<Int32> source, PonderableNode<Int32> next, PonderableNode<Int32> destination, GameMap map) {
                var weightByPossibleDamage = new DamageOnRouteCalculator(map).GetMaxSumDamageOnRoute(next.GetAscendantLocations());
                var weightByBarriers = GetWeightByBarriers(next.Location, destination.Location, map);
                return weightByPossibleDamage + weightByBarriers;
            }

            private Int32 GetWeightByBarriers(Location next, Location destination, GameMap map) =>
                map.GetMapElementsAround(next).Sum(info => GetWeightByBarriers(info.Item1, info.Item2, destination));
            private Int32 GetWeightByBarriers(Location aroundElementLocation, MapCellType aroundElementCellType, Location destination) {
                if(aroundElementCellType == MapCellType.HealthPack && aroundElementLocation != destination)
                    return 3;
                if(aroundElementCellType == MapCellType.Monster)
                    return 20;
                if(aroundElementCellType == MapCellType.Hidden)
                    return -2;
                if(aroundElementCellType == MapCellType.Item)
                    return -3;
                if(aroundElementCellType == MapCellType.Wall)
                    return -1;
                if(aroundElementCellType == MapCellType.Trap)
                    return -10;
                return -1;
            }
        }
    }
}
