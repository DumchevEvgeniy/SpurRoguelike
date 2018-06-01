using System;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;
using SpurRoguelike.WebPlayerBot.Targets;

internal sealed class StateMovementOnPosition : BaseState<TargetMovementOnPosition> {
    public StateMovementOnPosition(TargetMovementOnPosition target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        if(!playerGameInfo.GameMap.DetectedMonsters.IsEmpty()) {
            if(playerGameInfo.TargetAttackTheEnemy.MoveNext())
                return new StateAttackTheEnemy(playerGameInfo.TargetAttackTheEnemy, playerGameInfo);

            if(playerGameInfo.TargetTakeHealthPack.MoveNext())
                return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);

            if(playerGameInfo.TargetMovementOnTheMonster.MoveNext())
                return new StateMovementOnTheMonster(playerGameInfo.TargetMovementOnTheMonster, playerGameInfo);
        }
        if(playerGameInfo.TargetOpenMap.IsMaxOpen)
            return GetNextStateWhenMapIsOpen();
        return GetNextStateWhenMapIsNotOpen();
    }

    private Boolean CanDeath(Location nextLocation) {
        var monsters = playerGameInfo.GameMap.GetMapElementsAround(nextLocation).
            Where(info => info.Item2 == MapCellType.Monster)
            .Select(info => playerGameInfo.GameMap.DetectedMonsters.First(m => m.Location == info.Item1));
        var damageByPlayer = monsters.Sum(monster => monster.GetMaxDamageTo(playerGameInfo.GameMap.AreaInfo.Player));
        return damageByPlayer >= playerGameInfo.GameMap.AreaInfo.Player.Health;
    }


    private ITargetState<BaseTarget> GetNextStateWhenMapIsNotOpen() {
        if(NeedTakeHealthPack() && playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        if(playerGameInfo.TargetTakeBestItem.MoveNext())
            return new StateTakeBestItem(playerGameInfo.TargetTakeBestItem, playerGameInfo);
        if(playerGameInfo.TargetOpenMap.MoveNext())
            return new StateOpenMap(playerGameInfo.TargetOpenMap, playerGameInfo);
        currentTarget.MoveNext();
        return this;
    }

    private ITargetState<BaseTarget> GetNextStateWhenMapIsOpen() {
        if(playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        if(playerGameInfo.TargetTakeBestItemOnOpenMap.MoveNext())
            return new StateTakeBestItemOnOpenMap(playerGameInfo.TargetTakeBestItemOnOpenMap, playerGameInfo);
        playerGameInfo.TargetMovementToExitOnOpenMap.MoveNext();
        return new StateMovementToExitOnOpenMap(playerGameInfo.TargetMovementToExitOnOpenMap, playerGameInfo);
    }

    private Boolean NeedTakeHealthPack() =>
        playerGameInfo.GameMap.AreaInfo.Player.Health <= playerGameInfo.GameMap.MaxPlayerHealth * 0.4;

    private Boolean NeedMovementToExit() {
        if(playerGameInfo.GameMap.DetectedLocationsOfExits.IsEmpty())
            return false;
        if(playerGameInfo.GameMap.AreaInfo.Player.Health > playerGameInfo.GameMap.MaxPlayerHealth * 0.2)
            return false;
        return true;
    }
}

