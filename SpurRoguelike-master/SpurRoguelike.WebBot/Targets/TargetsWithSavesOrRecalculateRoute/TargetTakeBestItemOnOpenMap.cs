using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal sealed class TargetTakeBestItemOnOpenMap : BaseTargetWithSavesOrRecalculateRoute {
        private readonly IComparer<ItemViewInfo> defaultItemViewComparer = new ItemViewComparer();
        private IComparer<ItemViewInfo> CurrentItemViewComparer => ItemViewComparer ?? defaultItemViewComparer;

        public IComparer<ItemViewInfo> ItemViewComparer { get; set; }

        public TargetTakeBestItemOnOpenMap(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() => targetRoute == null || indexNextCell < targetRoute.Count - 1;

        protected override TargetRouteInfo TryGetRoute() {
            var safeMovement = CreateSafeMovement();
            var unsafeMovement = CreateUnsafeMovement();
            foreach(var bestItems in TryFoundBestItems()) {
                var safeRoute = safeMovement.GetRoute(gameMap.AreaInfo.Player.Location, bestItems.Location);
                if(safeRoute != null)
                    return new TargetRouteInfo(safeRoute, true);
                var unsafeRoute = unsafeMovement.GetRoute(gameMap.AreaInfo.Player.Location, bestItems.Location);
                if(unsafeRoute != null) {
                    var countTrapsOnRoute = unsafeRoute.Count(loc => gameMap[loc] == MapCellType.Trap);
                    if(countTrapsOnRoute * 50 >= gameMap.AreaInfo.Player.Health)
                        continue;
                    if(ExistHealthPacks(countTrapsOnRoute))
                        return new TargetRouteInfo(unsafeRoute, false);
                }
            }
            return null;
        }

        private IEnumerable<ItemViewInfo> TryFoundBestItems() {
            List<ItemViewInfo> foundedBestItems;
            if(gameMap.AreaInfo.Player.TryGetEquippedItem(out ItemViewInfo equippedItem)) {
                var bestItems = gameMap.DetectedItems.Where(el => CurrentItemViewComparer.Compare(el, equippedItem) > 0);
                if(bestItems.IsEmpty())
                    yield break;
                foundedBestItems = bestItems.ToList();
            }
            else
                foundedBestItems = gameMap.DetectedItems.ToList();
            while(foundedBestItems.Count != 0) {
                var bestItem = foundedBestItems.Aggregate((item1, item2) => CurrentItemViewComparer.Compare(item1, item2) > 0 ? item1 : item2);
                yield return bestItem;
                foundedBestItems.Remove(bestItem);
            }
        }

        private Boolean ExistHealthPacks(Int32 minCount) {
            Int32 counter = 0;
            foreach(var healthPack in gameMap.DetectedHealthPacks) {
                var routeSeacher = new RouteSeacher() {
                    Barriers = new List<MapCellType> { MapCellType.Trap, MapCellType.Wall, MapCellType.HealthPack, MapCellType.Exit, MapCellType.Monster },
                    Destination = healthPack.Location
                };
                var movement = new Movement<Double>(gameMap, routeSeacher, new WeightCalculatorForShortestMovement());
                var route = movement.GetRoute(gameMap.AreaInfo.Player.Location, healthPack.Location);
                if(route == null)
                    continue;
                counter++;
                if(counter == minCount)
                    return true;
            }
            return false;
        }

        private Movement<Int32> CreateSafeMovement() => new Movement<Int32>(gameMap, SafeRouteSeacher, new WeightCalculatorWithoutSelectionOfElements());
        private Movement<Int32> CreateUnsafeMovement() => new Movement<Int32>(gameMap, UnsafeRouteSeacher, new WeightCalculatorWithoutSelectionOfElements());

        private IRoute SafeRouteSeacher => new RouteSeacher() {
            Barriers = new List<MapCellType> { MapCellType.Trap, MapCellType.Wall, MapCellType.HealthPack, MapCellType.Exit, MapCellType.Monster }
        };
        private IRoute UnsafeRouteSeacher => new RouteSeacher() {
            Barriers = new List<MapCellType> { MapCellType.Wall, MapCellType.HealthPack, MapCellType.Exit, MapCellType.Monster }
        };
    }
}