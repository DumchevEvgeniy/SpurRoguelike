using System;
using System.Collections.Generic;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.PlayerBot.Game {
    internal class ItemViewComparer : IComparer<ItemView> {
        public Int32 Compare(ItemView firstItem, ItemView secondItem) {
            var sumBonusCompareResult = (firstItem.AttackBonus + firstItem.DefenceBonus).CompareTo(secondItem.AttackBonus + secondItem.DefenceBonus);
            if(sumBonusCompareResult == 0) {
                var attackCompareResult = firstItem.AttackBonus.CompareTo(secondItem.AttackBonus);
                if(attackCompareResult == 0)
                    return firstItem.DefenceBonus.CompareTo(secondItem.DefenceBonus);
                return attackCompareResult;
            }
            return sumBonusCompareResult;
        }
    }
}