using System;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Targets;

internal sealed class StateTakeBestItem : BaseState<TargetTakeBestItem> {
    public StateTakeBestItem(TargetTakeBestItem target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        if(NeedTakeHealthPack() && playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        if(currentTarget.MoveNext())
            return this;
        if(playerGameInfo.TargetOpenMap.IsMaxOpen)
            return GetNextStateWhenMapIsOpen();
        return GetNextStateWhenMapIsNotOpen();
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
        if(playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        if(NeedMovementToExit() && playerGameInfo.TargetMovementToExit.MoveNext())
            return new StateMovementToExit(playerGameInfo.TargetMovementToExit, playerGameInfo);
        playerGameInfo.TargetOpenMap.MoveNext();
        return new StateOpenMap(playerGameInfo.TargetOpenMap, playerGameInfo);
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
}

