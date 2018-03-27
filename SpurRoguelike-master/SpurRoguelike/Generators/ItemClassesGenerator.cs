using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Generators {
    internal class ItemClassesGenerator {
        public ItemClassesGenerator(Int32 seed, NameGenerator nameGenerator) {
            this.nameGenerator = nameGenerator;

            random = new Random(seed);
        }

        public List<ItemClass> Generate(Int32 variance, params ItemClassOptions[] itemClassOptions) {
            var itemClasses = new List<ItemClass>();

            foreach(var options in itemClassOptions) {
                var classesToGenerate = Math.Max(1, (Int32)Math.Round(Math.Sqrt(random.NextDouble()) * variance));

                for(Int32 i = 0; i < classesToGenerate; i++) {
                    var name = nameGenerator.Generate();
                    GenerateItemStats(options.Level, out var attackBonus, out var defenceBonus);

                    itemClasses.Add(new ItemClass(() => new Item(name, attackBonus, defenceBonus), options.Rarity, options.Level));
                }
            }

            return itemClasses;
        }

        private void GenerateItemStats(Int32 level, out Int32 attackBonus, out Int32 defenceBonus) {
            var bonus = random.Next(1, level + 1);

            attackBonus = (Int32)(random.NextDouble() * bonus);
            defenceBonus = bonus - attackBonus;
        }

        private readonly NameGenerator nameGenerator;
        private readonly Random random;
    }
}