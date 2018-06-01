using System;

namespace SpurRoguelike.WebPlayerBot {
    public class OpenApiInfo {
        public const String None = "api/spur/none";

        public const String StepToTheBottom = "api/spur/step/south";
        public const String StepToTheLeft = "api/spur/step/west";
        public const String StepToTheRight = "api/spur/step/east";
        public const String StepToTheTop = "api/spur/step/north";

        public const String AttackToTheBottom = "api/spur/attack/south";
        public const String AttackToTheLeft = "api/spur/attack/west";
        public const String AttackToTheRight = "api/spur/attack/east";
        public const String AttackToTheTop = "api/spur/attack/north";
        public const String AttackToTheRightBottomCorner = "api/spur/attack/southeast";
        public const String AttackToTheRightTopCorner = "api/spur/attack/northeast";
        public const String AttackToTheLeftBottomCorner = "api/spur/attack/southwest";
        public const String AttackToTheLeftTopCorner = "api/spur/attack/northwest";

        public const String Start = "api/spur/start";

        public const String View = "api/spur/view";
    }
}