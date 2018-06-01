using SpurRoguelike.PlayerBot.Extensions;
using SpurRoguelike.PlayerBot.Targets;

internal sealed class StateOpenMap : BaseState<TargetOpenMap> {
    public StateOpenMap(TargetOpenMap target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        if(!playerGameInfo.GameMap.DetectedMonsters.IsEmpty())
            return GetNextStateWhenMonsterExist();
        if(playerGameInfo.TargetOpenMap.IsMaxOpen)
            return GetNextStateWhenMapIsOpen();
        return GetNextStateWhenMapIsNotOpen();
    }

    private ITargetState<BaseTarget> GetNextStateWhenMonsterExist() {
        if(playerGameInfo.TargetAttackTheEnemy.MoveNext())
            return new StateAttackTheEnemy(playerGameInfo.TargetAttackTheEnemy, playerGameInfo);
        if(playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        if(playerGameInfo.TargetMovementOnTheMonster.MoveNext())
            return new StateMovementOnTheMonster(playerGameInfo.TargetMovementOnTheMonster, playerGameInfo);
        if(currentTarget.MoveNext())
            return this;
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
        if(playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        currentTarget.MoveNext();
        return this;
    }
}
