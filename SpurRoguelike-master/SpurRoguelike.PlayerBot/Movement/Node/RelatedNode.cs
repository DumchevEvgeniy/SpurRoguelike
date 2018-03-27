using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;

internal class RelatedNode : Node {
    public Node PreviousNode { get; set; }
    public Node NextNode { get; set; }

    public RelatedNode(Int32 x, Int32 y) : base(x, y) { }
    public RelatedNode(Location location) : base(location) { }

    public IEnumerable<Location> GetAscendantLocations() {
        RelatedNode current = this;
        while(current.PreviousNode != null) {
            var previousNode = current.PreviousNode as RelatedNode;
            previousNode.NextNode = current;
            current = previousNode;
        }
        while(true) {
            yield return current.Location;
            current = current.NextNode as RelatedNode;
            if(current.NextNode == null) {
                yield return current.Location;
                yield break;
            }
        }
    }
}