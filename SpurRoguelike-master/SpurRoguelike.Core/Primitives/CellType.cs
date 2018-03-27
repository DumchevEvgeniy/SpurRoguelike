using System;

namespace SpurRoguelike.Core.Primitives {
    public enum CellType : Byte {
        Wall = 0,
        Trap = 1,
        PlayerStart = 2,
        Exit = 3,
        Empty = 4,
        Hidden = 5
    }
}