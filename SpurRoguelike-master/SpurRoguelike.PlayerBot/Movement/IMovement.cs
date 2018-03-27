using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;

internal interface IMovement {
    IEnumerable<Location> GetRoute(Location sourceLocation, Location destinationLocation);
}
