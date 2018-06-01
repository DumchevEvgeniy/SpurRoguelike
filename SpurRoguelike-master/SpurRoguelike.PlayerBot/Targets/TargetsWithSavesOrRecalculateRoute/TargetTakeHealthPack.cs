using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal sealed class TargetTakeHealthPack : BaseTargetMovementOnBestPositionWhenMonsters {
        public TargetTakeHealthPack(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() {
            if(gameMap.DetectedHealthPacks.IsEmpty() || gameMap.AreaInfo.Player.Health == gameMap.MaxPlayerHealth)
                return false;
            if(RouteExist && gameMap[targetRoute.Last()] != MapCellType.HealthPack)
                return false;
            return true;
        }

        protected override IEnumerable<Location> GetTargetLocations() => GetHealthPacks().Select(hp => hp.Location);

        private IOrderedEnumerable<HealthPackView> GetHealthPacks() =>
            gameMap.DetectedHealthPacks.OrderBy(m => (m.Location - gameMap.AreaInfo.Player.Location).Size());

        protected override IEnumerable<MapCellType> Barriers =>
            new List<MapCellType> { MapCellType.Wall, MapCellType.Exit, MapCellType.HealthPack, MapCellType.Hidden, MapCellType.Trap, MapCellType.Monster, MapCellType.Item };

        protected override IWeightCalculator<Int32> AdditionalWeightCalculator => new WeightCalculator();

        private class WeightCalculator : IWeightCalculator<Int32> {
            public Int32 GetWeight(PonderableNode<Int32> source, PonderableNode<Int32> next, PonderableNode<Int32> destination, GameMap map) =>
                GetWeightByNeighborMonster(next.Location, map);
            
            public Int32 GetWeightByNeighborMonster(Location next, GameMap gameMap) {
                if(!gameMap.GetMapElementsAround(gameMap.AreaInfo.Player.Location).Any(info => info.Item1 == next))
                    return 0;
                var analyzedLocation = GetAnalyzedLocation(next, gameMap);
                if(!gameMap.Contains(analyzedLocation))
                    return 0;
                if(gameMap[analyzedLocation] != MapCellType.Monster)
                    return 0;
                var monster = gameMap.DetectedMonsters.First(m => m.Location == analyzedLocation);
                return monster.GetMaxDamageTo(gameMap.AreaInfo.Player);
            }

            private Location GetAnalyzedLocation(Location next, GameMap gameMap) {
                var offset = (gameMap.AreaInfo.Player.Location - next);
                if(offset.XOffset == 0)
                    return new Location(next.X, (next - offset).Y);
                return new Location((next - offset).X, next.Y);
            }
        }
    }
}