using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Extensions;
using SpurRoguelike.PlayerBot.Game;

namespace SpurRoguelike.PlayerBot.Targets {
    internal sealed class TargetOpenMap : BaseTargetWithSavesOrRecalculateRoute {
        private Boolean[,] reachabilityMap;
        private Location? lastTargetLocation;

        public Boolean IsMaxOpen { get; private set; } = false;

        public TargetOpenMap(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() => !IsMaxOpen;

        protected override TargetRouteInfo TryGetRoute() {
            if(lastTargetLocation != null && gameMap[lastTargetLocation.Value] == MapCellType.Hidden) {
                var safeMovement = CreateSafeMovement().GetRoute(gameMap.AreaInfo.Player.Location, lastTargetLocation.Value);
                if(safeMovement != null)
                    return new TargetRouteInfo(safeMovement, true);
                var unsafeMovement = CreateUnsafeMovement().GetRoute(gameMap.AreaInfo.Player.Location, lastTargetLocation.Value);
                if(unsafeMovement != null)
                    return new TargetRouteInfo(unsafeMovement, true);
            }
            InitializeReachabilityMap();
            var route = SeachOpeningRoute();
            if(route == null)
                IsMaxOpen = true;
            if(route != null)
                lastTargetLocation = route.Last();
            return new TargetRouteInfo(route, true);
        }

        private void InitializeReachabilityMap() => reachabilityMap = new Boolean[gameMap.AreaInfo.MapWidth, gameMap.AreaInfo.MapHeight];

        private IEnumerable<Location> SeachOpeningRoute() {
            var bordersHiddenCells = GetBordersHiddenCells();
            if(bordersHiddenCells.IsEmpty())
                return null;
            IEnumerable<Location> moreSafelyRoute = null;
            var safeMovement = CreateSafeMovement();
            var unsafeMovement = CreateUnsafeMovement();
            foreach(var hiddenLocation in GetOptimalRouteMapOpening(bordersHiddenCells))
                foreach(var freeLocationsOnQuadrant in GetFreeLocationsOnQuadrants(hiddenLocation)) {
                    Boolean needMiss = false;
                    foreach(var freeLocation in freeLocationsOnQuadrant.OrderBy(loc => (hiddenLocation - loc).Size()).ToList()) {
                        if(needMiss && reachabilityMap[freeLocation.X, freeLocation.Y])
                            continue;
                        if(!reachabilityMap[freeLocation.X, freeLocation.Y]) {
                            var safeRoute = safeMovement.GetRoute(gameMap.AreaInfo.Player.Location, freeLocation);
                            if(safeRoute != null)
                                return safeRoute;
                            MarkUnattainableCell(freeLocation);
                        }
                        var unsafeRoute = unsafeMovement.GetRoute(gameMap.AreaInfo.Player.Location, freeLocation);
                        if(unsafeRoute == null) {
                            needMiss = true;
                            continue;
                        }
                        if(moreSafelyRoute == null) {
                            moreSafelyRoute = unsafeRoute;
                            continue;
                        }
                        if(GetDamageByPlayer(unsafeRoute) < GetDamageByPlayer(moreSafelyRoute))
                            moreSafelyRoute = unsafeRoute;
                    }
                }
            if(moreSafelyRoute == null || PlayerCanBeDestroyed(moreSafelyRoute))
                return null;
            return moreSafelyRoute;
        }
        private IEnumerable<Location> GetOptimalRouteMapOpening(IEnumerable<Location> bordersHiddenCells) {
            var startPosition = lastTargetLocation != null && lastTargetLocation.HasValue ? lastTargetLocation.Value : gameMap.AreaInfo.Player.Location;
            var bordersHiddenCellsList = bordersHiddenCells.ToList();
            while(bordersHiddenCellsList.Count != 0) {
                var nextLocation = bordersHiddenCellsList.Aggregate((loc1, loc2) =>
                    (startPosition - loc1).Size() < (startPosition - loc2).Size() ? loc1 : loc2);
                yield return nextLocation;
                startPosition = nextLocation;
                bordersHiddenCellsList.Remove(nextLocation);
            }
        }
        private IEnumerable<Location> GetBordersHiddenCells() =>
            gameMap.GetLocationByTypes(MapCellType.Hidden).Where(loc => OnBorderWithOpenTerritory(loc));

        private void MarkUnattainableCell(Location unattainableLocation) {
            reachabilityMap[unattainableLocation.X, unattainableLocation.Y] = true;
            var unattainableLocations = new List<Location>() { unattainableLocation };
            while(unattainableLocations.Count != 0) {
                var tempLocationList = new List<Location>();
                foreach(var location in unattainableLocations) {
                    var items = new List<Location> {
                    new Location(location.X, location.Y - 1),
                    new Location(location.X - 1, location.Y),
                    new Location(location.X + 1, location.Y),
                    new Location(location.X, location.Y + 1)
                }.Where(loc => !reachabilityMap[loc.X, loc.Y] && !gameMap[loc].OneFrom(MapCellType.Exit, MapCellType.Hidden, MapCellType.Wall));

                    foreach(var item in items) {
                        reachabilityMap[item.X, item.Y] = true;
                        tempLocationList.Add(item);
                    }
                }
                unattainableLocations = tempLocationList;
            }
        }

        private IEnumerable<IEnumerable<Location>> GetFreeLocationsOnQuadrants(Location hiddenLocation) {
            return GetQuadrants(hiddenLocation)
                .Select(quadrant => gameMap.GetElementsByRectangle(quadrant.LeftTopCorner, quadrant.RightBottomCorner)
                .Where(info => !info.Item2.OneFrom(MapCellType.Hidden, MapCellType.Exit, MapCellType.Wall, MapCellType.Monster)
                    && !reachabilityMap[info.Item1.X, info.Item1.Y]
                    && info.Item1 != gameMap.AreaInfo.Player.Location)
                .Select(info => info.Item1));
        }
        private IEnumerable<Quadrant> GetQuadrants(Location targetLocation) {
            return targetLocation.GetQuadrants(gameMap.AreaInfo.VisibilityWidth, gameMap.AreaInfo.VisibilityHeight)
                .OrderBy(quadrant => GetSumDistance(quadrant));
        }

        private Boolean OnBorderWithOpenTerritory(Location location) => gameMap.GetMapElementsAround(location).Any(info => info.Item2 != MapCellType.Hidden);
        private Boolean PlayerCanBeDestroyed(IEnumerable<Location> moreSafelyRoute) => GetDamageByPlayer(moreSafelyRoute) >= gameMap.AreaInfo.Player.Health;
        private Int32 GetDamageByPlayer(IEnumerable<Location> route) =>
            route.Count(loc => gameMap[loc] == MapCellType.Trap) * GameMap.DamageByTrap;
        private Int32 GetSumDistance(Quadrant quadrant) =>
            quadrant.GetAllCorners().Sum(el => (el - gameMap.AreaInfo.Player.Location).Size());

        private Movement<Int32> CreateSafeMovement() => new Movement<Int32>(gameMap, safeRouteSeacher, weightCalculator);
        private Movement<Int32> CreateUnsafeMovement() => new Movement<Int32>(gameMap, unsafeRouteSeacher, weightCalculator);

        private IRoute safeRouteSeacher = new RouteSeacher() { Barriers = new List<MapCellType> { MapCellType.Hidden, MapCellType.Exit, MapCellType.Wall, MapCellType.Monster, MapCellType.Trap } };
        private IRoute unsafeRouteSeacher = new RouteSeacher() { Barriers = new List<MapCellType> { MapCellType.Hidden, MapCellType.Exit, MapCellType.Wall, MapCellType.Monster } };
        private IWeightCalculator<Int32> weightCalculator = new WeightCalculatorWithoutSelectionOfElements();
    }
}