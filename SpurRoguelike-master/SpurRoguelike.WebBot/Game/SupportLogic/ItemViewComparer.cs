using System;
using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal class ItemViewComparer : IComparer<ItemViewInfo> {
        public Int32 Compare(ItemViewInfo firstItem, ItemViewInfo secondItem) {
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