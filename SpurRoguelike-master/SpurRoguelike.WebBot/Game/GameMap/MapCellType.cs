using System;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal enum MapCellType : Byte {
        Hidden = 0,
        None = 1,
        Wall = 2,
        Trap = 3,
        Exit = 4,
        Player = 5,
        Monster = 6,
        Item = 7,
        HealthPack = 8,
    }
}