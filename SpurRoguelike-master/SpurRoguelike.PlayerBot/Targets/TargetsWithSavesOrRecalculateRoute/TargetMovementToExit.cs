using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Extensions;
using SpurRoguelike.PlayerBot.Game;

namespace SpurRoguelike.PlayerBot.Targets {
    internal sealed class TargetMovementToExit : BaseTargetMovementOnBestPositionWhenMonsters {
        public TargetMovementToExit(GameMap gameMap) : base(gameMap) { }

        public override Boolean IsAvailable() => !gameMap.DetectedLocationsOfExits.IsEmpty();

        protected override IEnumerable<Location> GetTargetLocations() => gameMap.DetectedLocationsOfExits.OrderBy(el => (gameMap.AreaInfo.Player.Location - el).Size());

        protected override IEnumerable<MapCellType> Barriers => new List<MapCellType> { MapCellType.Wall, MapCellType.Hidden };
    }
}