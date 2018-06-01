using System;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

internal class WeightCalculatorWithoutSelectionOfElements : IWeightCalculator<Int32> {
    public Int32 GetWeight(PonderableNode<Int32> source, PonderableNode<Int32> next, PonderableNode<Int32> destination, GameMap map) =>
        GetWeightByCellType(map[next.Location]) + GetWeightByManhattan(source.Location, next.Location) + GetWeightByShortestDistance(next.Location, destination.Location);

    public Int32 GetWeightByCellType(MapCellType elementCellType) {
        if(elementCellType == MapCellType.HealthPack)
            return 70;
        if(elementCellType == MapCellType.Item)
            return 50;
        if(elementCellType == MapCellType.Trap)
            return 150;
        return 0;
    }

    public Int32 GetWeightByManhattan(Location from, Location to) => (from - to).Size();

    public Int32 GetWeightByShortestDistance(Location from, Location to) {
        var offset = (from - to).Abs();
        return (Int32)Math.Sqrt(Math.Pow(offset.XOffset, 2) + Math.Pow(offset.YOffset, 2));
    }
}
