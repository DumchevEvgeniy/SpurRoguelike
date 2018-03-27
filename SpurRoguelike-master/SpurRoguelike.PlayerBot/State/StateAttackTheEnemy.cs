using System;
using SpurRoguelike.PlayerBot.Extensions;
using SpurRoguelike.PlayerBot.Targets;

internal sealed class StateAttackTheEnemy : BaseState<TargetAttackTheEnemy> {
    public StateAttackTheEnemy(TargetAttackTheEnemy target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    private void ResetSavedStates() {
        playerGameInfo.TargetMovementOnPosition.Reset();
        playerGameInfo.TargetMovementOnTheMonster.Reset();
        playerGameInfo.TargetMovementToExit.Reset();
        playerGameInfo.TargetOpenMap.Reset();
        playerGameInfo.TargetTakeBestItem.Reset();
        playerGameInfo.TargetTakeHealthPack.Reset();
    }

    public override ITargetState<BaseTarget> NextState() {
        ResetSavedStates();

        if(!playerGameInfo.GameMap.DetectedMonsters.IsEmpty()) {
            if(currentTarget.MoveNext())
                return this;
            if(NeedTakeHealthPack() && playerGameInfo.TargetTakeHealthPack.MoveNext())
                return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
            if(playerGameInfo.TargetMovementOnTheMonster.MoveNext())
                return new StateMovementOnTheMonster(playerGameInfo.TargetMovementOnTheMonster, playerGameInfo);
        }
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
        if(playerGameInfo.TargetTakeBestItem.MoveNext())
            return new StateTakeBestItem(playerGameInfo.TargetTakeBestItem, playerGameInfo);
        playerGameInfo.TargetOpenMap.MoveNext();
        return new StateOpenMap(playerGameInfo.TargetOpenMap, playerGameInfo);
    }

    private Boolean NeedTakeHealthPack() =>
        playerGameInfo.GameMap.AreaInfo.Player.Health <= playerGameInfo.GameMap.MaxPlayerHealth * 0.4;
}
