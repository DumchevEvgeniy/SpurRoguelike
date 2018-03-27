using System;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Extensions;

internal class Node {
    public Location Location { get; private set; }

    public Node(Int32 x, Int32 y) {
        Location = new Location(x, y);
    }
    public Node(Location location) : this(location.X, location.Y) { }

    public Int32 GetOffsetByX(Node other) => other.Location.X - Location.X;
    public Int32 GetOffsetByY(Node other) => other.Location.Y - Location.Y;

    public Boolean IsHorizontalWith(Node other) => other.Location.Y == Location.Y;
    public Boolean IsVerticalWith(Node other) => other.Location.X == Location.X;

    public Double GetShortestDistance(Node other) {
        if(IsHorizontalWith(other))
            return GetOffsetByY(other).Abs();
        if(IsVerticalWith(other))
            return GetOffsetByX(other).Abs();
        return NumberExtensions.GetHypotenuse(GetOffsetByY(other), GetOffsetByX(other));
    }

    public override Boolean Equals(Object obj) {
        if(obj == null)
            return false;
        var cell = obj as Node;
        if(cell == null)
            return false;
        return Location == cell.Location;
    }
    public override Int32 GetHashCode() {
        return Location.Y | Location.X;
    }
}