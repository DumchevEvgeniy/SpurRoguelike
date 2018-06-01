using System;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal class TurnInfo {
        public TurnType TurnType { get; private set; }

        public TurnInfo(TurnType turnType) {
            TurnType = turnType;
        }

        public static TurnInfo Create(Offset offset, Boolean isStep) {
            if(offset.XOffset == 0 && offset.YOffset == 0)
                return new TurnInfo(TurnType.None);
            return new TurnInfo(isStep ? Turn.Step(offset) : Turn.Attack(offset), offset.ToTurnType(isStep));
        }
    }
}