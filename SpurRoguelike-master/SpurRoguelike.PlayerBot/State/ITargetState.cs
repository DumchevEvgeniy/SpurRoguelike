using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Targets;

internal interface ITargetState<out TState> where TState : BaseTarget {
    ITargetState<BaseTarget> NextState();

    TurnInfo CurrentTurn { get; }
}
