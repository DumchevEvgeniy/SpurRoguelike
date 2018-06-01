using System;
using SpurRoguelike.WebPlayerBot.Game;

namespace SpurRoguelike.WebPlayerBot {
    public static class TurnTypeExtensions {
        public static String ToApi(this TurnType turnType) {
            switch (turnType) {
                case TurnType.None: return OpenApiInfo.None;

                case TurnType.StepToTheBottom: return OpenApiInfo.StepToTheBottom;
                case TurnType.StepToTheLeft: return OpenApiInfo.StepToTheLeft;
                case TurnType.StepToTheRight: return OpenApiInfo.StepToTheRight;
                case TurnType.StepToTheTop: return OpenApiInfo.StepToTheTop;

                case TurnType.AttackToTheBottom: return OpenApiInfo.AttackToTheBottom;
                case TurnType.AttackToTheLeft: return OpenApiInfo.AttackToTheLeft;
                case TurnType.AttackToTheRight: return OpenApiInfo.AttackToTheRight;
                case TurnType.AttackToTheTop: return OpenApiInfo.AttackToTheTop;
                case TurnType.AttackToTheRightBottomCorner: return OpenApiInfo.AttackToTheRightBottomCorner;
                case TurnType.AttackToTheRightTopCorner: return OpenApiInfo.AttackToTheRightTopCorner;
                case TurnType.AttackToTheLeftBottomCorner: return OpenApiInfo.AttackToTheLeftBottomCorner;
                case TurnType.AttackToTheLeftTopCorner: return OpenApiInfo.AttackToTheLeftTopCorner;
            }
            return OpenApiInfo.None;
        }
    }
}