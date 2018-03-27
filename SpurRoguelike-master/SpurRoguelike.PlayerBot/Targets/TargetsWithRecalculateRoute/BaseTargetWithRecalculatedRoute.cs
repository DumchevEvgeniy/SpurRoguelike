using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Game;

namespace SpurRoguelike.PlayerBot.Targets {
    internal abstract class BaseTargetWithRecalculateRoute : BaseTarget {
        protected BaseTargetWithRecalculateRoute(GameMap gameMap) : base(gameMap) { }

        public override Boolean MoveNext() {
            if(!IsAvailable())
                return false;
            var route = TryGetRoute();
            if(route == null)
                return false;
            current = TurnInfo.Create(route.Skip(1).First() - gameMap.AreaInfo.Player.Location, true);
            return true;
        }

        protected abstract IEnumerable<Location> TryGetRoute();
    }
}