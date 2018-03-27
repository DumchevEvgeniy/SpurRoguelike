using SpurRoguelike.PlayerBot.Game;
using SpurRoguelike.PlayerBot.Targets;

internal abstract class BaseState<TTarget> : ITargetState<TTarget> where TTarget : BaseTarget {
    protected readonly TTarget currentTarget;
    protected readonly PlayerGameInfo playerGameInfo;

    protected BaseState(TTarget target, PlayerGameInfo playerGameInfo) {
        currentTarget = target;
        this.playerGameInfo = playerGameInfo;
    }

    public TurnInfo CurrentTurn => currentTarget.Current;

    public abstract ITargetState<BaseTarget> NextState();
}

