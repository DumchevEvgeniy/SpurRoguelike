using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;
using SpurRoguelike.WebPlayerBot.Extensions;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal class DamageOnRouteCalculator {
        private readonly GameMap gameMap;

        public DamageOnRouteCalculator(GameMap gameMap) {
            this.gameMap = gameMap;
        }

        public Int32 GetMaxSumDamageOnRoute(IEnumerable<Location> route, Int32 limitSumDamage = Int32.MaxValue) {
            if(route == null || route.IsEmpty() || gameMap.DetectedMonsters.IsEmpty())
                return 0;
            var routeList = route.ToList();
            var nearbyMonsters = gameMap.DetectedMonsters.Where(monster => InTheZoneOfVisibility(monster.Location, route.Last(), routeList.Count));
            Int32 sumDamageResult = GetDamageOnRoute(routeList);
            if(sumDamageResult > limitSumDamage)
                return Int32.MaxValue;
            foreach(var monster in nearbyMonsters) {
                sumDamageResult += GetMonsterDamage(monster, routeList) * 2;
                if(sumDamageResult > limitSumDamage)
                    return Int32.MaxValue;
            }
            return sumDamageResult;
        }

        private Int32 GetDamageOnRoute(List<Location> routeList) {
            Int32 sumDamage = 0;
            for(Int32 indexCell = 1; indexCell < routeList.Count - 1; indexCell++) {
                var typeCell = gameMap[routeList[indexCell]];
                if(typeCell == MapCellType.Trap)
                    sumDamage += 50;
                if(typeCell == MapCellType.HealthPack)
                    sumDamage -= 50;
            }
            return sumDamage;
        }

        private Int32 GetMonsterDamage(PawnView monster, List<Location> routeList) {
            var monsterRouteCalculator = new ShortestMonsterRouteCalculator(gameMap);
            Int32 counterPlayerSteps = 0;
            for(Int32 index = 0; index < routeList.Count - 1; index++) {
                counterPlayerSteps++;
                var nextPlayerLocation = routeList[index + 1];
                var previousPlayerLocation = routeList[index];

                monsterRouteCalculator.Calculate(monster.Location, nextPlayerLocation, previousPlayerLocation, counterPlayerSteps);
                if(monsterRouteCalculator.RouteExist)
                    return monster.GetMaxDamageTo(gameMap.AreaInfo.Player);

                if(gameMap[nextPlayerLocation].OneFrom(MapCellType.HealthPack, MapCellType.Item, MapCellType.Trap))
                    counterPlayerSteps++;
            }
            return 0;
        }

        private Boolean InTheZoneOfVisibility(Location pawnLocation, Location center, Int32 radius) =>
            Math.Pow(center.X - pawnLocation.X, 2) + Math.Pow(center.Y - pawnLocation.Y, 2) <= Math.Pow(radius, 2) + 1;

        private class ShortestMonsterRouteCalculator {
            private readonly GameMap gameMap;

            public IEnumerable<Location> ShortestMonsterRoute { get; private set; }
            public Int32 CounterMonsterSteps { get; private set; } = Int32.MaxValue;
            public Boolean RouteExist => ShortestMonsterRoute != null;

            public ShortestMonsterRouteCalculator(GameMap gameMap) {
                this.gameMap = gameMap;
            }

            public void Calculate(Location monsterLocation, Location playerDestenition, Location previousPlayerLocation, Int32 maxCountSteps) {
                var targetMonsterDestenitionLocations = gameMap.GetMapElementsAround(playerDestenition)
                    .Where(info => (info.Item1 - monsterLocation).Size() <= maxCountSteps && info.Item2 != MapCellType.Wall && info.Item1 != previousPlayerLocation)
                    .Select(info => info.Item1)
                    .OrderBy(loc => (loc - monsterLocation).Size());
                if(targetMonsterDestenitionLocations.IsEmpty())
                    return;
                ShortestMonsterRoute = null;
                CounterMonsterSteps = Int32.MaxValue;
                foreach(var targetMonsterLocation in targetMonsterDestenitionLocations) {
                    var route = CreateMonsterMovement(monsterLocation, targetMonsterLocation, maxCountSteps).GetRoute(monsterLocation, targetMonsterLocation);
                    if(route == null)
                        continue;
                    var numberOfVisitedLocations = GetMinNumberOfVisitedLocations(route, maxCountSteps, CounterMonsterSteps);

                    if(ShortestMonsterRoute == null || numberOfVisitedLocations < CounterMonsterSteps) {
                        CounterMonsterSteps = numberOfVisitedLocations;
                        ShortestMonsterRoute = route;
                    }
                }
            }

            private Int32 GetMinNumberOfVisitedLocations(IEnumerable<Location> route, Int32 maxCountStepsLimit, Int32 lastMinNumberOfVisitedLocations) {
                Int32 stepsCounter = 0;
                foreach(var location in route.Skip(1)) {
                    stepsCounter += gameMap[location].OneFrom(MapCellType.HealthPack, MapCellType.Item, MapCellType.Trap) ? 2 : 1;
                    if(stepsCounter > lastMinNumberOfVisitedLocations || stepsCounter > maxCountStepsLimit)
                        return Int32.MaxValue;
                }
                return stepsCounter;
            }

            private Boolean InTheZoneOfVisibility(Location pawnLocation, Location center, Int32 radius) =>
                Math.Pow(center.X - pawnLocation.X, 2) + Math.Pow(center.Y - pawnLocation.Y, 2) <= Math.Pow(radius, 2) + 1;

            private IMovement CreateMonsterMovement(Location monsterLocation, Location destination, Int32 radius) {
                var routeSeacher = new RouteSeacherBuilder()
                    .AddDestination(destination)
                    .AddBarriers(MapCellType.Wall)
                    .AddPredicateOnAvailability((location, map) => InTheZoneOfVisibility(monsterLocation, destination, radius))
                    .Create();
                return new Movement<Double>(gameMap, routeSeacher, new WeightCalculatorForShortestMovement());
            }
        }
    }
}