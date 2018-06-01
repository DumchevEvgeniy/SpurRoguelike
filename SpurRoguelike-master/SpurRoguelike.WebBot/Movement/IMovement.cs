using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Infractructure;

internal interface IMovement {
    IEnumerable<Location> GetRoute(Location sourceLocation, Location destinationLocation);
}
