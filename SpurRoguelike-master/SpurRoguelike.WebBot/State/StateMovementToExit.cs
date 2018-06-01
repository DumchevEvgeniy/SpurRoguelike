using SpurRoguelike.PlayerBot.Targets;

internal sealed class StateMovementToExit : BaseState<TargetMovementToExit> {
    public StateMovementToExit(TargetMovementToExit target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        if(playerGameInfo.TargetTakeHealthPack.MoveNext())
            return new StateTakeHealthPack(playerGameInfo.TargetTakeHealthPack, playerGameInfo);
        currentTarget.MoveNext();
        return this;
    }
}

