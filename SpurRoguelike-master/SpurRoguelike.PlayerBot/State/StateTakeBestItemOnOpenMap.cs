using SpurRoguelike.WebPlayerBot.Targets;

internal sealed class StateTakeBestItemOnOpenMap : BaseState<TargetTakeBestItemOnOpenMap> {
    public StateTakeBestItemOnOpenMap(TargetTakeBestItemOnOpenMap target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        if(currentTarget.MoveNext())
            return this;
        playerGameInfo.TargetMovementToExitOnOpenMap.MoveNext();
        return new StateMovementToExitOnOpenMap(playerGameInfo.TargetMovementToExitOnOpenMap, playerGameInfo);
    }
}

