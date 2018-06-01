using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.WebPlayerBot.Game;

namespace SpurRoguelike.WebPlayerBot.Targets {
    internal abstract class BaseTargetWithSavesOrRecalculateRoute : BaseTarget {
        protected List<Location> targetRoute;
        protected Int32 indexNextCell = 0;

        protected BaseTargetWithSavesOrRecalculateRoute(GameMap gameMap) : base(gameMap) { }

        public Boolean RouteExist => targetRoute != null && indexNextCell < targetRoute.Count - 1 && targetRoute[indexNextCell] == gameMap.AreaInfo.Player.Location;

        public override void Reset() {
            targetRoute = null;
            indexNextCell = 0;
            ResetValues();
            base.Reset();
        }

        protected virtual void ResetValues() { }

        public override Boolean MoveNext() {
            if(!IsAvailable())
                return false;
            if(!RouteExist || gameMap[targetRoute[indexNextCell + 1]].OneFrom(MapCellType.Monster, MapCellType.Wall)) {
                var routeInfo = TryGetRoute();
                if(routeInfo == null || routeInfo.Route == null)
                    return false;
                if(!routeInfo.NeedSave) {
                    current = TurnInfo.Create(routeInfo.Route.Skip(1).First() - gameMap.AreaInfo.Player.Location, true);
                    return true;
                }
                targetRoute = routeInfo.Route.ToList();
                indexNextCell = 0;
            }
            var nextPlayerLocation = targetRoute[indexNextCell + 1];
            if(!gameMap[nextPlayerLocation].OneFrom(MapCellType.HealthPack, MapCellType.Item, MapCellType.Trap))
                indexNextCell++;
            current = TurnInfo.Create(nextPlayerLocation - gameMap.AreaInfo.Player.Location, true);
            return true;
        }

        protected abstract TargetRouteInfo TryGetRoute();
    }
}
