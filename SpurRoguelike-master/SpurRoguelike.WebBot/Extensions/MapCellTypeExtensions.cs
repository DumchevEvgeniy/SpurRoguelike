using System;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

internal static class MapCellTypeExtensions {
    public static Boolean OneFrom(this MapCellType element, params MapCellType[] elements) =>
        elements == null ? false : elements.Any(item => item == element);

    public static MapCellType ToMapCellType(this CellType cellType) {
        switch(cellType) {
            case CellType.Wall:
                return MapCellType.Wall;
            case CellType.Trap:
                return MapCellType.Trap;
            case CellType.PlayerStart:
                return MapCellType.None;
            case CellType.Exit:
                return MapCellType.Exit;
            case CellType.Empty:
                return MapCellType.None;
            default:
                return MapCellType.Hidden;
        }
    }
}
