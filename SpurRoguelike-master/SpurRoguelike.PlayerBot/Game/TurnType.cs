using System;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal enum TurnType : Byte {
        None = 0,

        StepToTheLeft = 1,
        StepToTheRight = 2,
        StepToTheTop = 3,
        StepToTheBottom = 4,

        AttackToTheLeft = 5,
        AttackToTheRight = 6,
        AttackToTheTop = 7,
        AttackToTheBottom = 8,
        AttackToTheLeftTopCorner = 9,
        AttackToTheRightTopCorner = 10,
        AttackToTheLeftBottomCorner = 11,
        AttackToTheRightBottomCorner = 12,
    }
}