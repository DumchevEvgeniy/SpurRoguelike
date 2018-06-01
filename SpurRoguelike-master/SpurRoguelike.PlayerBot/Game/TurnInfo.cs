using System;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.WebPlayerBot.Extensions;

namespace SpurRoguelike.WebPlayerBot.Game {
    internal class TurnInfo {
        public Turn Turn { get; private set; }
        public TurnType TurnType { get; private set; }

        public TurnInfo(Turn turn, TurnType turnType) {
            Turn = turn;
            TurnType = turnType;
        }

        public static TurnInfo Create(Offset offset, Boolean isStep) {
            if(offset.XOffset == 0 && offset.YOffset == 0)
                return new TurnInfo(Turn.None, TurnType.None);
            return new TurnInfo(isStep ? Turn.Step(offset) : Turn.Attack(offset), offset.ToTurnType(isStep));
        }
    }
}