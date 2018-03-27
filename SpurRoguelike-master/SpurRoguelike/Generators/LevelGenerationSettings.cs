using System;

namespace SpurRoguelike.Generators {
    internal class LevelGenerationSettings {
        public FieldOptions Field { get; set; }

        public TrapOptions Traps { get; set; }

        public ItemOptions Items { get; set; }

        public HealthPackOptions HealthPacks { get; set; }

        public MonsterOptions Monsters { get; set; }

        public class FieldOptions {
            public Int32 MinWidth { get; set; }
            public Int32 MaxWidth { get; set; }
            public Int32 MinHeight { get; set; }
            public Int32 MaxHeight { get; set; }
            public Int32 MinVisibilityWidth { get; set; }
            public Int32 MaxVisibilityWidth { get; set; }
            public Int32 MinVisibilityHeight { get; set; }
            public Int32 MaxVisibilityHeight { get; set; }
            public Double FreeSpaceShare { get; set; }
        }

        public class TrapOptions {
            public Double Density { get; set; }
        }

        public class ItemOptions {
            public Double Density { get; set; }
            public Int32 MinLevel { get; set; }
            public Int32 MaxLevel { get; set; }
        }

        public class HealthPackOptions {
            public Double Density { get; set; }
        }

        public class MonsterOptions {
            public Double Density { get; set; }
            public Double MinSkill { get; set; }
            public Double MaxSkill { get; set; }
        }
    }
}