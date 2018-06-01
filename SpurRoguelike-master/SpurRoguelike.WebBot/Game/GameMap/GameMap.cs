﻿using System;
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

        private readonly MapHealthPacksUpdater healthPacksUpdater = new MapHealthPacksUpdater();
        private readonly MapItemsUpdater itemsUpdater = new MapItemsUpdater();
        private readonly MapMonsterUpdater monsterUpdater = new MapMonsterUpdater();
        private readonly MapWallsUpdater wallsUpdater = new MapWallsUpdater();
        private readonly MapTrapsUpdater trapsUpdater = new MapTrapsUpdater();
        private readonly List<Location> detectedLocationsOfExits = new List<Location>();

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
        public GameMap(FieldView fieldView, PawnViewInfo player, Int32 maxPlayerHealth)
            : this(fieldView.Width, fieldView.Height, fieldView.VisibilityWidth, fieldView.VisibilityHeight, player, maxPlayerHealth) {
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
        private void SetMapCellValuesByColumn(Location topBorder, Location bottomBorder, Func<Location, MapCellType> func) {
            for(Int32 y = topBorder.Y; y <= bottomBorder.Y; y++) {
                var location = new Location { X = topBorder.X, Y = y };
                this[location] = func(location);
            }
        }
        private void SetMapCellValuesByRectangle(Location leftTopCorner, Location rightBottomCorner, Func<Location, MapCellType> func) {
            for(Int32 y = leftTopCorner.Y; y <= rightBottomCorner.Y; y++)
                SetMapCellValuesByRow(new Location { X = leftTopCorner.X, Y = y }, new Location { X = rightBottomCorner.X, Y = y }, func);
        }

        public IEnumerable<Tuple<Location, MapCellType>> GetElementsByRow(Location leftBorder, Location rightBorder) {
            if(leftBorder.Y != rightBorder.Y || leftBorder.Y >= areaInfo.MapHeight || leftBorder.Y < 0)
                yield break;
            if(leftBorder.X > rightBorder.X || leftBorder.X >= areaInfo.MapWidth || rightBorder.X < 0)
                yield break;
            var realLeftBorder = leftBorder.X < 0 ? new Location { X = 0, Y = leftBorder.Y } : leftBorder;
            var realRightBorder = rightBorder.X >= areaInfo.MapWidth ? new Location { X = areaInfo.MapWidth - 1, Y = rightBorder.Y } : rightBorder;
            for(Int32 x = realLeftBorder.X; x <= realRightBorder.X; x++) {
                var location = new Location { X = x, Y = realLeftBorder.Y };
                yield return Tuple.Create(location, this[location]);
            }
        }
        public IEnumerable<Tuple<Location, MapCellType>> GetElementsByColumn(Location topBorder, Location bottomBorder) {
            if(topBorder.X != bottomBorder.X || topBorder.X >= areaInfo.MapWidth || topBorder.X < 0)
                yield break;
            if(topBorder.Y > bottomBorder.Y || topBorder.Y < 0 || bottomBorder.Y >= areaInfo.MapHeight)
                yield break;
            var realTopBorder = topBorder.Y < 0 ? new Location { X = topBorder.X, Y = 0 } : topBorder;
            var realBottomBorder = bottomBorder.Y >= areaInfo.MapHeight ? new Location { X = bottomBorder.X, Y = areaInfo.MapHeight - 1 } : bottomBorder;
            for(Int32 y = realTopBorder.Y; y <= realBottomBorder.Y; y++) {
                var location = new Location { X = realTopBorder.X, Y = y };
                yield return Tuple.Create(location, this[location]);
            }
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

        public IEnumerable<PawnViewInfo> DetectedMonsters => monsterUpdater.DetectedElements;
        public IEnumerable<ItemViewInfo> DetectedItems => itemsUpdater.DetectedElements;
        public IEnumerable<HealthPackViewInfo> DetectedHealthPacks => healthPacksUpdater.DetectedElements;
        public IEnumerable<Location> DetectedTraps => trapsUpdater.ElementsLocations;
        public IEnumerable<Location> DetectedWalls => wallsUpdater.ElementsLocations;
        public IEnumerable<Location> DetectedLocationsOfExits => detectedLocationsOfExits;
        public VisibleAreaInfo AreaInfo => areaInfo;


        private abstract class BaseMapElementUpdater<T> {
            protected List<Location> elementsLocations = new List<Location>();
            public IEnumerable<Location> ElementsLocations => elementsLocations;

            protected BaseMapElementUpdater() { }

            public abstract void Update(GameMap gameMap, IEnumerable<T> visibleElements);
            protected abstract MapCellType ToMapCellType();
        }
        private abstract class MapElementUpdaterByLocation : BaseMapElementUpdater<Location> {
            protected MapElementUpdaterByLocation() : base() { }

            public override void Update(GameMap gameMap, IEnumerable<Location> visibleElements) {
                foreach(var location in elementsLocations.Where(location => gameMap.areaInfo.InTheVisibleArea(location)).ToList()) {
                    elementsLocations.Remove(location);
                    gameMap[location] = MapCellType.None;
                }
                foreach(var location in visibleElements) {
                    elementsLocations.Add(location);
                    gameMap[location] = ToMapCellType();
                }
            }
        }
        private abstract class MapElementUpdater<T> : BaseMapElementUpdater<T> {
            protected readonly List<T> detectedElements = new List<T>();
            public IEnumerable<T> DetectedElements => detectedElements;

            protected MapElementUpdater() : base() { }

            public override void Update(GameMap gameMap, IEnumerable<T> visibleElements) {
                RemoveFromMap(gameMap, elementsLocations);

                RemoveDestroyedElements(gameMap);

                AddSetElements(visibleElements);
                elementsLocations = detectedElements.Select(el => ToLocation(el)).ToList();
                SetCellTypeByLocations(gameMap, elementsLocations, ToMapCellType());
            }

            protected abstract Location ToLocation(T element);
            protected abstract void RemoveDestroyedElements(GameMap gameMap);

            private void AddSetElements(IEnumerable<T> sender) {
                foreach(var element in sender)
                    if(!detectedElements.Contains(element))
                        detectedElements.Add(element);
            }
            private void RemoveFromMap(GameMap gameMap, IEnumerable<Location> locations) {
                if(locations != null)
                    SetCellTypeByLocations(gameMap, locations, MapCellType.None);
            }
            private void SetCellTypeByLocations(GameMap gameMap, IEnumerable<Location> locations, MapCellType cellType) {
                foreach(var location in locations)
                    gameMap[location] = cellType;
            }
        }

        private class MapWallsUpdater : MapElementUpdaterByLocation {
            public MapWallsUpdater() : base() { }

            protected override MapCellType ToMapCellType() => MapCellType.Wall;
        }
        private class MapTrapsUpdater : MapElementUpdaterByLocation {
            public MapTrapsUpdater() : base() { }

            protected override MapCellType ToMapCellType() => MapCellType.Trap;
        }
        private class MapItemsUpdater : MapElementUpdater<ItemViewInfo> {
            public MapItemsUpdater() : base() { }

            protected override void RemoveDestroyedElements(GameMap gameMap) {
                elementsLocations?.RemoveAll(location => gameMap.areaInfo.InTheVisibleArea(location));
                detectedElements.RemoveAll(el => gameMap.areaInfo.InTheVisibleArea(el.Location));
            }
            protected override Location ToLocation(ItemViewInfo element) => element.Location;
            protected override MapCellType ToMapCellType() => MapCellType.Item;
        }
        private class MapHealthPacksUpdater : MapElementUpdater<HealthPackViewInfo> {
            public MapHealthPacksUpdater() : base() { }

            protected override void RemoveDestroyedElements(GameMap gameMap) {
                elementsLocations?.RemoveAll(location => gameMap.areaInfo.InTheVisibleArea(location));
                detectedElements.RemoveAll(el => gameMap.areaInfo.InTheVisibleArea(el.Location));
            }
            protected override Location ToLocation(HealthPackViewInfo element) => element.Location;
            protected override MapCellType ToMapCellType() => MapCellType.HealthPack;
        }
        private class MapMonsterUpdater : MapElementUpdater<PawnViewInfo> {
            public MapMonsterUpdater() : base() { }

            protected override void RemoveDestroyedElements(GameMap gameMap) => detectedElements.RemoveAll(el => el == null);
            protected override Location ToLocation(PawnViewInfo element) => element.Location;
            protected override MapCellType ToMapCellType() => MapCellType.Monster;
        }




        public struct VisibleAreaInfo {
            public PawnViewInfo Player { get; internal set; }
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