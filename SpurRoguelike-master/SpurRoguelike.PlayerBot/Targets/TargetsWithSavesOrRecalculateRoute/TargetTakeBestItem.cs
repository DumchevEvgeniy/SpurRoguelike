using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal sealed class TargetTakeBestItem : BaseTargetWithSavesOrRecalculateRoute {
        private IEnumerable<ItemView> bestItems;
        private readonly IComparer<ItemView> defaultItemViewComparer = new ItemViewComparer();
        private IComparer<ItemView> CurrentItemViewComparer => ItemViewComparer ?? defaultItemViewComparer;

        public IComparer<ItemView> ItemViewComparer { get; set; }

        public TargetTakeBestItem(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() {
            if(gameMap.DetectedItems.IsEmpty())
                return false;
            bestItems = TryFoundBestItems();
            if(bestItems == null || bestItems.IsEmpty())
                return false;
            return true;
        }

        protected override TargetRouteInfo TryGetRoute() {
            var foundedBestItems = bestItems.ToList();
            while(foundedBestItems.Count != 0) {
                var bestItem = foundedBestItems.Aggregate((item1, item2) => CurrentItemViewComparer.Compare(item1, item2) > 0 ? item1 : item2);
                var safeRoute = CreateMovement(bestItem.Location).GetRoute(gameMap.AreaInfo.Player.Location, bestItem.Location);
                if(safeRoute != null) {
                    var currentSumDamage = new DamageOnRouteCalculator(gameMap).GetMaxSumDamageOnRoute(safeRoute);
                    if(currentSumDamage <= 0)
                        return new TargetRouteInfo(safeRoute, true);
                    return new TargetRouteInfo(safeRoute, false);
                }
                foundedBestItems.Remove(bestItem);
            }
            return null;
        }

        private IEnumerable<ItemView> TryFoundBestItems() {
            if(gameMap.AreaInfo.Player.TryGetEquippedItem(out ItemView equippedItem)) {
                var bestItems = gameMap.DetectedItems.Where(el => CurrentItemViewComparer.Compare(el, equippedItem) > 0);
                if(bestItems.IsEmpty())
                    return null;
                return bestItems;
            }
            return gameMap.DetectedItems;
        }

        private IMovement CreateMovement(Location destination) {
            var routeSeacher = new RouteSeacher {
                Destination = destination,
                Barriers = new List<MapCellType> { MapCellType.Wall, MapCellType.Trap, MapCellType.Exit, MapCellType.Item, MapCellType.Hidden }
            };
            return new Movement<Int32>(gameMap, routeSeacher, new WeightCalculatorForDifficultSituation());
        }

        private sealed class WeightCalculatorForDifficultSituation : IWeightCalculator<Int32> {
            public Int32 GetWeight(PonderableNode<Int32> source, PonderableNode<Int32> next, PonderableNode<Int32> destination, GameMap map) {
                var weightByPossibleDamage = new DamageOnRouteCalculator(map).GetMaxSumDamageOnRoute(next.GetAscendantLocations());
                var weightByCellType = GetWeightByCellType(map[next.Location]);
                var weightByManhattan = GetWeightByManhattan(source.Location, next.Location);
                var weightByShortestDistance = GetWeightByShortestDistance(next.Location, destination.Location);
                var weightByBarriers = GetWeightByBarriers(next.Location, destination.Location, map);
                var weightByMonsters = GetWeightByMonsters(next.Location, map);
                return weightByPossibleDamage + weightByCellType + weightByManhattan + weightByShortestDistance + weightByBarriers - weightByMonsters;
            }

            public Int32 GetWeightByCellType(MapCellType elementCellType) {
                if(elementCellType == MapCellType.HealthPack)
                    return 70;
                if(elementCellType == MapCellType.Item)
                    return 50;
                if(elementCellType == MapCellType.Trap)
                    return 150;
                return 0;
            }

            public Int32 GetWeightByManhattan(Location from, Location to) => (from - to).Size();

            public Int32 GetWeightByMonsters(Location next, GameMap gameMap) =>
                gameMap.DetectedMonsters.Sum(m => GetWeightByManhattan(m.Location, next));

            public Int32 GetWeightByShortestDistance(Location from, Location to) {
                var offset = (from - to).Abs();
                return (Int32)Math.Sqrt(Math.Pow(offset.XOffset, 2) + Math.Pow(offset.YOffset, 2));
            }

            private Int32 GetWeightByBarriers(Location next, Location destination, GameMap map) => map.GetMapElementsAround(next).Sum(info => GetWeightByBarriers(info.Item1, info.Item2, destination));
            private Int32 GetWeightByBarriers(Location aroundElementLocation, MapCellType aroundElementCellType, Location destination) {
                if(aroundElementCellType == MapCellType.HealthPack && aroundElementLocation != destination)
                    return 3;
                if(aroundElementCellType == MapCellType.Monster)
                    return 30;
                if(aroundElementCellType == MapCellType.Hidden)
                    return -2;
                if(aroundElementCellType == MapCellType.Item)
                    return -3;
                if(aroundElementCellType == MapCellType.Wall)
                    return -1;
                if(aroundElementCellType == MapCellType.Trap)
                    return -10;
                if(aroundElementCellType == MapCellType.None)
                    return -2;
                return 0;
            }
        }
    }
}
