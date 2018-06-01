using System;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal sealed class GameMap {
        public const Int32 DamageByTrap = 50;
        public const Int32 DamageByHealthPack = -50;

        private readonly MapCellType[,] cells;
        private VisibleAreaInfo areaInfo;

        private readonly List<Location> detectedLocationsOfExits = new List<Location>();

        public IEnumerable<PawnViewInfo> DetectedMonsters { get; } = new List<PawnViewInfo>();
        public IEnumerable<ItemViewInfo> DetectedItems { get; } = new List<ItemViewInfo>();
        public IEnumerable<HealthPackViewInfo> DetectedHealthPacks { get; } = new List<HealthPackViewInfo>();
        public IEnumerable<Location> DetectedTraps { get; } = new List<Location>();
        public IEnumerable<Location> DetectedWalls { get; } = new List<Location>();
        public IEnumerable<Location> DetectedLocationsOfExits { get; } = new List<Location>();
        public VisibleAreaInfo AreaInfo => areaInfo;

        public Int32 MaxPlayerHealth { get; private set; }

        public GameMap(Int32 width, Int32 height, Int32 visibilityWidth, Int32 visibilityHeight, PawnViewInfo player, Int32 maxPlayerHealth) {
            areaInfo = new VisibleAreaInfo() {
                Player = player,
                MapWidth = width,
                MapHeight = height,
                VisibilityWidth = Math.Min(visibilityWidth, width),
                VisibilityHeight = Math.Min(visibilityHeight, height)
            };
            this.MaxPlayerHealth = maxPlayerHealth;
            cells = new MapCellType[width, height];
        }
        public GameMap(LevelDataViewInfo levelData, PawnViewInfo player, Int32 maxPlayerHealth)
            : this(levelData.Field.Width, levelData.Field.Height, levelData.Field.VisibilityWidth, levelData.Field.VisibilityHeight, player, maxPlayerHealth) {
        }

        public MapCellType this[Location location] {
            get => this[location.X, location.Y];
            private set => this[location.X, location.Y] = value;
        }
        public MapCellType this[Int32 x, Int32 y] {
            get => cells[x, y];
            private set => cells[x, y] = value;
        }

        public Boolean Contains(Location location) =>
            location.X >= 0 && location.X < areaInfo.MapWidth && location.Y >= 0 && location.Y < areaInfo.MapHeight;

        public void UpdateState(LevelViewInfo levelViewInfo) {
            AreaInfo.Player = levelViewInfo.LevelData.Player;
            ExploreNewTerrain(levelViewInfo);

            //monsterUpdater.Update(this, levelViewInfo.Monsters);
            //itemsUpdater.Update(this, levelViewInfo.Items);
            //healthPacksUpdater.Update(this, levelViewInfo.HealthPacks);
            //wallsUpdater.Update(this, levelViewInfo.Field.GetCellsOfType(CellType.Wall));
            //trapsUpdater.Update(this, levelViewInfo.Field.GetCellsOfType(CellType.Trap));
        }

        private void ExploreNewTerrain(LevelViewInfo levelViewInfo) {
            var leftTopCorner = areaInfo.GetLeftTopCorner();
            var rightBottomCorner = areaInfo.GetRightBottomCorner();
            SetMapCellValuesByRectangle(leftTopCorner, rightBottomCorner, (location) => SetMapCellType(location, levelViewInfo));
        }

        private void SetMapCellValuesByRow(Location leftBorder, Location rightBorder, Func<Location, MapCellType> func) {
            for(Int32 x = leftBorder.X; x <= rightBorder.X; x++) {
                var location = new Location { X = x, Y = leftBorder.Y };
                this[location] = func(location);
            }
        }
        private void SetMapCellValuesByRectangle(Location leftTopCorner, Location rightBottomCorner, Func<Location, MapCellType> func) {
            for(Int32 y = leftTopCorner.Y; y <= rightBottomCorner.Y; y++)
                SetMapCellValuesByRow(new Location { X = leftTopCorner.X, Y = y }, new Location { X = rightBottomCorner.X, Y = y }, func);
        }

        public IEnumerable<Tuple<Location, MapCellType>> GetElementsByRectangle(Location leftTopCorner, Location rightBottomCorner) {
            if(leftTopCorner.X >= areaInfo.MapWidth || leftTopCorner.Y >= areaInfo.MapHeight)
                yield break;
            if(leftTopCorner.X > rightBottomCorner.X || leftTopCorner.Y > rightBottomCorner.Y)
                yield break;
            var realLeftTopCorner = new Location { X = Math.Max(0, leftTopCorner.X), Y = Math.Max(0, leftTopCorner.Y) };
            var realRightBottomCorner = new Location { X = Math.Min(areaInfo.MapWidth - 1, rightBottomCorner.X), Y = Math.Min(areaInfo.MapHeight - 1, rightBottomCorner.Y) };
            for(Int32 x = realLeftTopCorner.X; x <= realRightBottomCorner.X; x++) {
                for(Int32 y = realLeftTopCorner.Y; y <= realRightBottomCorner.Y; y++) {
                    var location = new Location { X = x, Y = y };
                    yield return Tuple.Create(location, this[location]);
                }
            }
        }

        public IEnumerable<Location> GetLocationByTypes(params MapCellType[] mapCellTypes) {
            if(mapCellTypes == null || mapCellTypes.IsEmpty())
                yield break;
            for(Int32 y = 0; y < areaInfo.MapHeight; y++)
                for(Int32 x = 0; x < areaInfo.MapWidth; x++)
                    if(this[x, y].OneFrom(mapCellTypes))
                        yield return new Location { X = x, Y = y };
        }

        private MapCellType SetMapCellType(Location location, LevelViewInfo levelViewInfo) {
            if(levelViewInfo.Field[location] == CellType.Exit && !detectedLocationsOfExits.Contains(location))
                detectedLocationsOfExits.Add(location);
            return levelViewInfo.Field[location].ToMapCellType();
        }

        public class VisibleAreaInfo {
            public PawnViewInfo Player { get; set; }
            public Int32 VisibilityWidth { get; internal set; }
            public Int32 VisibilityHeight { get; internal set; }
            public Int32 MapWidth { get; internal set; }
            public Int32 MapHeight { get; internal set; }

            public Location GetLeftTopCorner() => ToLocationOnMap(Player.Location.X - VisibilityWidth, Player.Location.Y - VisibilityHeight);
            public Location GetRightTopCorner() => ToLocationOnMap(Player.Location.X + VisibilityWidth, Player.Location.Y - VisibilityHeight);
            public Location GetLeftBottomCorner() => ToLocationOnMap(Player.Location.X - VisibilityWidth, Player.Location.Y + VisibilityHeight);
            public Location GetRightBottomCorner() => ToLocationOnMap(Player.Location.X + VisibilityWidth, Player.Location.Y + VisibilityHeight);

            public Boolean InTheVisibleArea(Location location) {
                var leftTopCorner = GetLeftTopCorner();
                var rightBottomCorner = GetRightBottomCorner();
                return location.X >= leftTopCorner.X && location.X <= rightBottomCorner.X &&
                    location.Y >= leftTopCorner.Y && location.Y <= rightBottomCorner.Y;
            }

            public Int32 SizeVisibilityWidth => VisibilityWidth * 2 + 1;
            public Int32 SizeVisibilityHeight => VisibilityHeight * 2 + 1;

            private Location ToLocationOnMap(Int32 x, Int32 y) {
                var correctX = Math.Min(Math.Max(0, x), MapWidth - 1);
                var correctY = Math.Min(Math.Max(0, y), MapHeight - 1);
                return new Location { X = correctX, Y = correctY };
            }
        }
    }
}