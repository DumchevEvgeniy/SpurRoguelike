using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.PlayerBot.Targets {
    internal class TargetRouteInfo {
        public Boolean NeedSave { get; private set; }
        public IEnumerable<Location> Route { get; private set; }

        public TargetRouteInfo(IEnumerable<Location> route, Boolean needSave) {
            Route = route;
            NeedSave = needSave;
        }
    }
}