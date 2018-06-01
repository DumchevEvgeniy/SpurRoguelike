using System;
using System.Collections.Generic;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal class MonsterComparer : IComparer<PawnViewInfo> {
        private PawnViewInfo player;

        public MonsterComparer(PawnViewInfo player) {
            this.player = player;
        }

        public Int32 Compare(PawnViewInfo monster1, PawnViewInfo monster2) {
            var damageToMonster1 = player.GetMaxDamageTo(monster1);
            var damageToMonster2 = player.GetMaxDamageTo(monster2);
            if(damageToMonster1 >= monster1.Health && damageToMonster2 >= monster2.Health)
                return monster1.Health.CompareTo(monster2.Health);
            if(damageToMonster1 >= monster1.Health)
                return -1;
            if(damageToMonster2 >= monster2.Health)
                return 1;
            if(monster1.Health / (Double)monster2.Health >= 1.75)
                return 1;
            if(monster2.Health / (Double)monster1.Health >= 1.75)
                return -1;
            var distanseToMonster1 = (monster1.Location - player.Location).Size();
            var distanseToMonster2 = (monster2.Location - player.Location).Size();
            if(distanseToMonster1 > distanseToMonster2 && monster1.TotalAttack / (Double)monster2.TotalAttack >= 1.5)
                return -1;
            if(distanseToMonster2 > distanseToMonster1 && monster2.TotalAttack / (Double)monster1.TotalAttack >= 1.5)
                return 1;
            return monster1.TotalAttack.CompareTo(monster2.TotalAttack);
        }
    }
}
