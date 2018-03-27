using SpurRoguelike.PlayerBot.Targets;

internal sealed class StateMovementToExitOnOpenMap : BaseState<TargetMovementToExitOnOpenMap> {
    public StateMovementToExitOnOpenMap(TargetMovementToExitOnOpenMap target, PlayerGameInfo playerGameInfo)
        : base(target, playerGameInfo) {
    }

    public override ITargetState<BaseTarget> NextState() {
        currentTarget.MoveNext();
        return this;
    }
}

