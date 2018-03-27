using System;
using System.Collections.Generic;

namespace SpurRoguelike.Generators {
    internal class MonsterClassesGenerator {
        public MonsterClassesGenerator(Int32 seed, NameGenerator nameGenerator) {
            this.nameGenerator = nameGenerator;

            random = new Random(seed);
        }

        public List<MonsterClass> Generate(Int32 variance, params MonsterClassOptions[] monsterClassOptions) {
            var classes = new List<MonsterClass>();

            foreach(var options in monsterClassOptions) {
                var classesToGenerate = Math.Max(1, (Int32)Math.Round(Math.Sqrt(random.NextDouble()) * variance));

                for(Int32 i = 0; i < classesToGenerate; i++) {
                    var name = nameGenerator.Generate();
                    var health = GenerateMonsterHealth(options.Skill);
                    var attack = GenerateMonsterStat(options.Skill);
                    var defence = GenerateMonsterStat(options.Skill);

                    classes.Add(new MonsterClass(() => options.Factory(name, options.Skill, health, attack, defence), options.Rarity, options.Skill));
                }
            }

            return classes;
        }

        private Int32 GenerateMonsterStat(Double skill) {
            return (Int32)(-3 + 40 * skill + -Math.Pow(random.NextDouble(), 0.1) * 10);
        }

        private Int32 GenerateMonsterHealth(Double skill) {
            return (Int32)((1 - Math.Pow(random.NextDouble(), 0.1)) * 100 + 200 * skill);
        }

        private readonly NameGenerator nameGenerator;
        private readonly Random random;
    }
}