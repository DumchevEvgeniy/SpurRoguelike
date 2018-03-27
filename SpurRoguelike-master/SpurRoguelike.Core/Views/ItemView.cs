using System;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views {
    public struct ItemView : IView {
        public ItemView(Item item) {
            this.item = item;
        }

        public Int32 AttackBonus => item?.AttackBonus ?? 0;

        public Int32 DefenceBonus => item?.DefenceBonus ?? 0;

        public Location Location => item?.Location ?? default(Location);

        public Boolean HasValue => item != null;

        private readonly Item item;
    }
}