using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal sealed class TargetAttackTheEnemy : BaseTarget {
        private IEnumerable<PawnViewInfo> allAroundMonsters;

        public TargetAttackTheEnemy(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() => NeedStrike();

        public override Boolean MoveNext() {
            if(!IsAvailable())
                return false;
            InitializeAroundMonsters();
            if(IsSurrounded()) {
                current = TurnInfo.Create(GetTargetMonster().Location - gameMap.AreaInfo.Player.Location, false);
                return true;
            }
            if(!CanStrike() || CanBeSurrounded())
                return false;
            var targetMonster = GetTargetMonster();
            if(!CanRunAfterStrike(targetMonster))
                return false;
            current = TurnInfo.Create(targetMonster.Location - gameMap.AreaInfo.Player.Location, false);
            return true;
        }

        private void InitializeAroundMonsters() {
            allAroundMonsters = gameMap.GetMapElementsAround(gameMap.AreaInfo.Player.Location)
                .Where(info => info.Item2 == MapCellType.Monster)
                .Select(info => gameMap.DetectedMonsters.First(m => m.Location == info.Item1));
        }

        private Boolean CanBeSurrounded() {
            var allAroundMonster = gameMap.GetMapElementsAround(gameMap.AreaInfo.Player.Location, 2)
                .Where(info => info.Item2 == MapCellType.Monster)
                .Select(info => gameMap.DetectedMonsters.First(m => m.Location == info.Item1));
            return allAroundMonster.Count() >= 4;
        }
        private Boolean CanRunAfterStrike(PawnViewInfo targetMonster) {
            var nextDamage = allAroundMonsters.Where(m => (m.Location - targetMonster.Location).Size() == 1)
                .Sum(monster => monster.GetMaxDamageTo(gameMap.AreaInfo.Player));
            return nextDamage * 0.8 < gameMap.AreaInfo.Player.Health * 0.75;
        }
        private Boolean CanStrike() {
            if(allAroundMonsters.IsEmpty())
                return false;
            var sumDamageToPlayer = allAroundMonsters.Sum(monster => monster.GetMaxDamageTo(gameMap.AreaInfo.Player));
            return sumDamageToPlayer < gameMap.AreaInfo.Player.Health * 0.5;
        }
        private Boolean IsSurrounded() {
            return gameMap.GetMapElementsAround(gameMap.AreaInfo.Player.Location)
                .Where(info => (info.Item1 - gameMap.AreaInfo.Player.Location).Size() == 1)
                .All(info => info.Item2.OneFrom(MapCellType.Monster, MapCellType.Wall, MapCellType.Trap));
        }
        private Boolean NeedStrike() {
            if(gameMap.DetectedMonsters.IsEmpty())
                return false;
            var aroundMonsterLocations = GetAroundMonsterLocations(1);
            if(aroundMonsterLocations.Count() != 1)
                return false;
            if(GetAroundMonsterLocations(2).Count() > 2)
                return false;
            var monster = gameMap.DetectedMonsters.First(m => m.Location == aroundMonsterLocations.First());
            return monster.GetMaxDamageTo(gameMap.AreaInfo.Player) * 4 < gameMap.AreaInfo.Player.Health;
        }

        private IEnumerable<Location> GetAroundMonsterLocations(Int32 radius) {
            return gameMap.GetMapElementsAround(gameMap.AreaInfo.Player.Location, radius).
                Where(info => info.Item2 == MapCellType.Monster)
                .Select(info => info.Item1);
        }

        private PawnViewInfo GetTargetMonster() =>
            allAroundMonsters.Aggregate((m1, m2) => new MonsterComparer(gameMap.AreaInfo.Player).Compare(m1, m2) <= 0 ? m1 : m2);
    }
}
