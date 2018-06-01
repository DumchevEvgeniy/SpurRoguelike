using System;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.WebPlayerBot.Game;

internal class WeightCalculatorForShortestMovement : IWeightCalculator<Double> {
    public Double GetWeight(PonderableNode<Double> source, PonderableNode<Double> next, PonderableNode<Double> destination, GameMap map) =>
        GetWeightByManhattan(source.Location, next.Location) + GetWeightByShortestDistance(next.Location, destination.Location);

    public Double GetWeightByManhattan(Location from, Location to) => (from - to).Size();

    public Double GetWeightByShortestDistance(Location from, Location to) {
        var offset = (from - to).Abs();
        return Math.Sqrt(Math.Pow(offset.XOffset, 2) + Math.Pow(offset.YOffset, 2));
    }
}