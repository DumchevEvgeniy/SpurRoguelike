using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal sealed class TargetMovementToExitOnOpenMap : BaseTargetWithSavesOrRecalculateRoute {
        private IComparer<IEnumerable<Location>> DefaultRouteComparer => new RouteComparerByTraps(gameMap);
        private IComparer<IEnumerable<Location>> CurrentRouteComparer => RouteComparer ?? DefaultRouteComparer;

        public IComparer<IEnumerable<Location>> RouteComparer { get; set; }

        public TargetMovementToExitOnOpenMap(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() {
            if(gameMap.DetectedLocationsOfExits.IsEmpty())
                return false;
            if(targetRoute != null && indexNextCell == targetRoute.Count - 1 && gameMap[targetRoute[indexNextCell]] == MapCellType.Exit)
                return false;
            return true;
        }

        protected override TargetRouteInfo TryGetRoute() {
            var safeRoute = TryGetFirstRightRoute(CreateSafeMovement(), gameMap.DetectedLocationsOfExits);
            if(safeRoute != null)
                return new TargetRouteInfo(safeRoute, true);

            var unsafeRoute = TryGetMoreSafetyRoute(CreateUnsafeMovement(), gameMap.DetectedLocationsOfExits);
            if(unsafeRoute != null)
                return new TargetRouteInfo(unsafeRoute, true);

            var safeNearestRoute = TryGetFirstRightNearestRoute();
            if(safeNearestRoute != null)
                return new TargetRouteInfo(safeNearestRoute, true);

            var unsafeNearestRoute = TryGetMoreSafetyNearestRoute();
            if(unsafeNearestRoute != null)
                return new TargetRouteInfo(unsafeNearestRoute, true);

            return null;
        }

        private IEnumerable<IEnumerable<Location>> GetNearestLocations() {
            foreach(var locationOfExit in gameMap.DetectedLocationsOfExits) {
                var quadrants = locationOfExit.GetQuadrants(gameMap.AreaInfo.VisibilityWidth, gameMap.AreaInfo.VisibilityHeight)
                    .Select(quadrant => gameMap.GetElementsByRectangle(quadrant.LeftTopCorner, quadrant.RightBottomCorner)
                    .Where(info => !info.Item2.OneFrom(MapCellType.Hidden, MapCellType.Wall, MapCellType.Monster) && info.Item1 != gameMap.AreaInfo.Player.Location)
                    .Select(info => info.Item1));
                foreach(var locations in quadrants)
                    yield return locations.OrderBy(loc => (locationOfExit - loc).Size());
            }
        }
        private IEnumerable<IEnumerable<Location>> TryGetRoutes(IMovement movement, IEnumerable<Location> destinationLocations) =>
            destinationLocations.Select(location => movement.GetRoute(gameMap.AreaInfo.Player.Location, location));

        private IEnumerable<Location> TryGetFirstRightNearestRoute() {
            foreach(var locations in GetNearestLocations()) {
                var safeRoute = TryGetFirstRightRoute(CreateSafeMovement(), locations);
                if(safeRoute != null)
                    return safeRoute;
            }
            return null;
        }
        private IEnumerable<Location> TryGetMoreSafetyNearestRoute() {
            List<IEnumerable<Location>> listResult = new List<IEnumerable<Location>>();
            foreach(var locations in GetNearestLocations()) {
                var safeRoute = TryGetFirstRightRoute(CreateSafeMovement(), locations);
                if(safeRoute != null)
                    listResult.Add(safeRoute);
            }
            if(listResult.Count == 0)
                return null;
            return listResult.Aggregate((r1, r2) => CurrentRouteComparer.Compare(r1, r2) <= 0 ? r1 : r2);
        }

        private IEnumerable<Location> TryGetFirstRightRoute(IMovement movement, IEnumerable<Location> destinationLocations) =>
            TryGetRoutes(movement, destinationLocations).FirstOrDefault(route => route != null);
        private IEnumerable<Location> TryGetRouteMoreOptimal(IMovement movement, IEnumerable<Location> destinationLocations,
            Func<IEnumerable<Location>, IEnumerable<Location>, IEnumerable<Location>> selectorOptimalRoute) =>
            TryGetRoutes(movement, destinationLocations)?.Aggregate(selectorOptimalRoute);
        private IEnumerable<Location> TryGetMoreSafetyRoute(IMovement movement, IEnumerable<Location> destinationLocations) =>
            TryGetRouteMoreOptimal(movement, destinationLocations, (r1, r2) => CurrentRouteComparer.Compare(r1, r2) <= 0 ? r1 : r2);

        private Movement<Int32> CreateSafeMovement() => new Movement<Int32>(gameMap, new RouteSeacher(), new WeightCalculatorWithoutSelectionOfElements());
        private Movement<Int32> CreateUnsafeMovement() => new Movement<Int32>(gameMap, UnsafeRouteSeacher, new WeightCalculatorWithoutSelectionOfElements());

        private IRoute UnsafeRouteSeacher => new RouteSeacher() {
            Barriers = new List<MapCellType> { MapCellType.Wall, MapCellType.Trap, MapCellType.Hidden }
        };
    }
}