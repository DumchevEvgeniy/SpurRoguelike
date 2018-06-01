using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal sealed class TargetMovementOnTheMonster : BaseTargetMovementOnBestPositionWhenMonsters {
        public TargetMovementOnTheMonster(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() {
            if(gameMap.DetectedMonsters.IsEmpty())
                return false;
            if(gameMap.GetMapElementsAround(gameMap.AreaInfo.Player.Location).Any(info => info.Item2 == MapCellType.Monster))
                return false;
            return true;
        }

        protected override IEnumerable<Location> GetTargetLocations() => GetMonsters().Select(m => m.Location);

        private IOrderedEnumerable<PawnView> GetMonsters() {
            var nearestMonster = gameMap.DetectedMonsters.Aggregate((m1, m2) =>
                (m1.Location - gameMap.AreaInfo.Player.Location).Size() <= (m2.Location - gameMap.AreaInfo.Player.Location).Size() ? m1 : m2);
            return gameMap.DetectedMonsters
                .Where(m => (m.Location - gameMap.AreaInfo.Player.Location).Size() - (nearestMonster.Location - gameMap.AreaInfo.Player.Location).Size() < 5)
                .OrderBy(m => (m.Location - gameMap.AreaInfo.Player.Location).Size())
                .ThenBy(m => m.Health)
                .ThenBy(m => m.TotalAttack)
                .ThenBy(m => m.TotalDefence);
        }

        protected override Boolean BestRouteIsAvailable(IEnumerable<Location> bestRoute, Int32 maxSumDamage) {
            if(gameMap.AreaInfo.Player.Health < maxSumDamage)
                return false;
            if(gameMap.GetMapElementsAround(bestRoute.Skip(1).First()).Count(el => el.Item2 == MapCellType.Monster) > 2)
                return false;
            return true;
        }

        protected override IEnumerable<MapCellType> Barriers => new List<MapCellType> { MapCellType.Hidden, MapCellType.Monster, MapCellType.Wall, MapCellType.Trap, MapCellType.Item };

        protected override IWeightCalculator<Int32> AdditionalWeightCalculator => new WeightCalculator();

        private class WeightCalculator : IWeightCalculator<Int32> {
            public Int32 GetWeight(PonderableNode<Int32> source, PonderableNode<Int32> next, PonderableNode<Int32> destination, GameMap map) {
                var weightByCellType = next.Location == destination.Location ? 0 : GetWeightByCellType(map[next.Location]);
                var weightByBarriers = GetWeightByBarriers(next.Location, destination.Location, map);
                var weightByManhattan = GetWeightByManhattan(next.Location, destination.Location);
                return weightByCellType + weightByBarriers + weightByManhattan;
            }

            public Int32 GetWeightByCellType(MapCellType elementCellType) {
                if(elementCellType == MapCellType.HealthPack)
                    return 40;
                if(elementCellType == MapCellType.Item)
                    return 20;
                if(elementCellType == MapCellType.Trap)
                    return 70;
                return 0;
            }

            public Int32 GetWeightByManhattan(Location from, Location to) => (from - to).Size();

            private Int32 GetWeightByBarriers(Location next, Location destination, GameMap map) => map.GetMapElementsAround(next).Sum(info => GetWeightByBarriers(info.Item1, info.Item2, destination));
            private Int32 GetWeightByBarriers(Location aroundElementLocation, MapCellType aroundElementCellType, Location destination) {
                if(aroundElementCellType == MapCellType.HealthPack && aroundElementLocation != destination)
                    return 3;
                if(aroundElementCellType == MapCellType.Monster)
                    return 90;
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