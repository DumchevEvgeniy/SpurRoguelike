using System;
using SpurRoguelike.Core.Entities;
using SpurRoguelike.Core.Primitives;

namespace SpurRoguelike.Core.Views {
    public struct PawnView : IView {
        public PawnView(Pawn pawn) {
            this.pawn = pawn;
        }

        public String Name => pawn?.Name;

        public Int32 Attack => pawn?.Attack ?? 0;

        public Int32 Defence => pawn?.Defence ?? 0;

        public Int32 TotalAttack => pawn?.TotalAttack ?? 0;

        public Int32 TotalDefence => pawn?.TotalDefence ?? 0;

        public Int32 Health => pawn?.Health ?? 0;

        public Boolean TryGetEquippedItem(out ItemView item) {
            if(pawn?.EquippedItem != null) {
                item = pawn.EquippedItem.CreateView();
                return true;
            }

            item = default(ItemView);
            return false;
        }

        public Boolean IsDestroyed => pawn?.IsDestroyed ?? false;

        public Location Location => pawn?.Location ?? default(Location);

        public Boolean HasValue => pawn != null;

        private readonly Pawn pawn;
    }
}