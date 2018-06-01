using SpurRoguelike.PlayerBot.Game;
using SpurRoguelike.PlayerBot.Targets;

internal interface ITargetState<out TState> where TState : BaseTarget {
    ITargetState<BaseTarget> NextState();

    TurnInfo CurrentTurn { get; }
}
