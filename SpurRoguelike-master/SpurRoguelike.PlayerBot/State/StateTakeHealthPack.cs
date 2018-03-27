using System;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Extensions;
using SpurRoguelike.PlayerBot.Game;
using SpurRoguelike.PlayerBot.Targets;

internal sealed class StateTakeHealthPack : BaseState<TargetTakeHealthPack> {
    public StateTakeHealthPack(TargetTakeHealthPack target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        if(!playerGameInfo.GameMap.DetectedMonsters.IsEmpty()) {
            if(NeedStrike() && playerGameInfo.TargetAttackTheEnemy.MoveNext())
                return new StateAttackTheEnemy(playerGameInfo.TargetAttackTheEnemy, playerGameInfo);

            if(NeedTakeHealthPack() && currentTarget.MoveNext())
                return this;

            if(playerGameInfo.TargetMovementOnTheMonster.MoveNext())
                return new StateMovementOnTheMonster(playerGameInfo.TargetMovementOnTheMonster, playerGameInfo);
        }
        if(playerGameInfo.TargetOpenMap.IsMaxOpen)
            return GetNextStateWhenMapIsOpen();
        return GetNextStateWhenMapIsNotOpen();
    }

    private ITargetState<BaseTarget> GetNextStateWhenMapIsOpen() {
        if(currentTarget.MoveNext())
            return this;
        if(playerGameInfo.TargetTakeBestItemOnOpenMap.MoveNext())
            return new StateTakeBestItemOnOpenMap(playerGameInfo.TargetTakeBestItemOnOpenMap, playerGameInfo);
        playerGameInfo.TargetMovementToExitOnOpenMap.MoveNext();
        return new StateMovementToExitOnOpenMap(playerGameInfo.TargetMovementToExitOnOpenMap, playerGameInfo);
    }
    private ITargetState<BaseTarget> GetNextStateWhenMapIsNotOpen() {
        if(currentTarget.MoveNext())
            return this;
        if(NeedMovementToExit() && playerGameInfo.TargetMovementToExit.MoveNext())
            return new StateMovementToExit(playerGameInfo.TargetMovementToExit, playerGameInfo);
        if(playerGameInfo.TargetOpenMap.MoveNext())
            return new StateOpenMap(playerGameInfo.TargetOpenMap, playerGameInfo);
        playerGameInfo.TargetMovementOnPosition.MoveNext();
        return new StateMovementOnPosition(playerGameInfo.TargetMovementOnPosition, playerGameInfo);
    }

    private Boolean NeedMovementToExit() {
        if(playerGameInfo.GameMap.DetectedLocationsOfExits.IsEmpty())
            return false;
        if(playerGameInfo.GameMap.AreaInfo.Player.Health > playerGameInfo.GameMap.MaxPlayerHealth * 0.1)
            return false;
        return true;
    }

    private Boolean NeedTakeHealthPack() =>
        playerGameInfo.GameMap.AreaInfo.Player.Health <= playerGameInfo.GameMap.MaxPlayerHealth * 0.4;

    private Boolean NeedStrike() {
        var locationsOfMonsters = playerGameInfo.GameMap.GetMapElementsAround(playerGameInfo.GameMap.AreaInfo.Player.Location, 2).
            Where(info => info.Item2 == MapCellType.Monster)
            .Select(info => info.Item1);
        if(locationsOfMonsters.Count() != 1)
            return false;
        return true;
    }
}
