using System;

namespace SpurRoguelike.PlayerBot.Extensions {
    internal static class NumberExtensions {
        public static Int32 Abs(this Int32 number) => Math.Abs(number);

        public static Double GetHypotenuse(Double firstCatheter, Double secondCatheter) =>
            Math.Sqrt(Math.Pow(firstCatheter, 2) + Math.Pow(secondCatheter, 2));
    }
}
