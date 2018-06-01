using System;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Targets;

internal sealed class StateMovementOnTheMonster : BaseState<TargetMovementOnTheMonster> {
    public StateMovementOnTheMonster(TargetMovementOnTheMonster target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        if(NeedTakeHealthPack() && playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        if(!playerGameInfo.GameMap.DetectedMonsters.IsEmpty())
            return GetNextStateWhenMonsterExist();
        if(playerGameInfo.TargetOpenMap.IsMaxOpen)
            return GetNextStateWhenMapIsOpen();
        return GetNextStateWhenMapIsNotOpen();
    }

    private ITargetState<BaseTarget> GetNextStateWhenMonsterExist() {
        if(playerGameInfo.TargetAttackTheEnemy.MoveNext())
            return new StateAttackTheEnemy(playerGameInfo.TargetAttackTheEnemy, playerGameInfo);
        if(currentTarget.MoveNext())
            return this;
        if(playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        playerGameInfo.TargetMovementOnPosition.MoveNext();
        return new StateMovementOnPosition(playerGameInfo.TargetMovementOnPosition, playerGameInfo);
    }
    private ITargetState<BaseTarget> GetNextStateWhenMapIsOpen() {
        if(playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        if(playerGameInfo.TargetTakeBestItemOnOpenMap.MoveNext())
            return new StateTakeBestItemOnOpenMap(playerGameInfo.TargetTakeBestItemOnOpenMap, playerGameInfo);
        playerGameInfo.TargetMovementToExitOnOpenMap.MoveNext();
        return new StateMovementToExitOnOpenMap(playerGameInfo.TargetMovementToExitOnOpenMap, playerGameInfo);
    }
    private ITargetState<BaseTarget> GetNextStateWhenMapIsNotOpen() {
        if(NeedMovementToExit() && playerGameInfo.TargetMovementToExit.MoveNext())
            return new StateMovementToExit(playerGameInfo.TargetMovementToExit, playerGameInfo);
        if(playerGameInfo.TargetOpenMap.MoveNext())
            return new StateOpenMap(playerGameInfo.TargetOpenMap, playerGameInfo);
        playerGameInfo.TargetMovementOnPosition.MoveNext();
        return new StateMovementOnPosition(playerGameInfo.TargetMovementOnPosition, playerGameInfo);
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
