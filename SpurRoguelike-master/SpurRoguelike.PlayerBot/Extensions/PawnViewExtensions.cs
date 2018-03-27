using System;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.PlayerBot.Extensions {
    internal static class PawnViewExtensions {
        private const Int32 BaseDamage = 10;

        public static Int32 GetMaxDamageTo(this PawnView instigator, PawnView victim) =>
            (Int32)((instigator.TotalAttack / (Double)victim.TotalDefence * BaseDamage));
    }
}