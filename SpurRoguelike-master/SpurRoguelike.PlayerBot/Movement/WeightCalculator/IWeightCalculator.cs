using System;
using SpurRoguelike.PlayerBot.Game;

internal interface IWeightCalculator<TWeight> where TWeight : IComparable<TWeight> {
    TWeight GetWeight(PonderableNode<TWeight> source, PonderableNode<TWeight> next, PonderableNode<TWeight> destination, GameMap map);
}