using System;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Extensions {
    internal static class PawnViewExtensions {
        private const Int32 BaseDamage = 10;

        public static Int32 GetMaxDamageTo(this PawnViewInfo instigator, PawnViewInfo victim) =>
            (Int32)((instigator.TotalAttack / (Double)victim.TotalDefence * BaseDamage));
    }
}