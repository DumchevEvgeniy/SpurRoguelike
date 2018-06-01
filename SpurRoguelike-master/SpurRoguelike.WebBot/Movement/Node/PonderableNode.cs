using System;
using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

internal class PonderableNode<TWeight> : RelatedNode, IComparable<TWeight>, IComparable<PonderableNode<TWeight>> where TWeight : IComparable<TWeight> {
    private IWeightCalculator<TWeight> weightCalculator;
    public IComparer<TWeight> WeightComparer { get; set; }
    public TWeight Weight { get; set; }

    public PonderableNode(Int32 x, Int32 y, IWeightCalculator<TWeight> weightCalculator, IComparer<TWeight> weightComparer)
        : base(x, y) {
        this.weightCalculator = weightCalculator;
        WeightComparer = weightComparer;
    }
    public PonderableNode(Int32 x, Int32 y, IWeightCalculator<TWeight> weightCalculator)
        : this(x, y, weightCalculator, null) { }
    public PonderableNode(Location location, IWeightCalculator<TWeight> weightCalculator, IComparer<TWeight> weightComparer)
        : this(location.X, location.Y, weightCalculator, weightComparer) { }
    public PonderableNode(Location location, IWeightCalculator<TWeight> weightCalculator)
        : this(location, weightCalculator, null) { }

    public TWeight CalculateWeight(PonderableNode<TWeight> next, PonderableNode<TWeight> destination, GameMap gameMap) =>
        weightCalculator.GetWeight(this, next, destination, gameMap);

    public Int32 CompareTo(TWeight otherWeight) =>
        WeightComparer == null ? Weight.CompareTo(otherWeight) : WeightComparer.Compare(Weight, otherWeight);

    public Int32 CompareTo(PonderableNode<TWeight> other) => CompareTo(other.Weight);
}