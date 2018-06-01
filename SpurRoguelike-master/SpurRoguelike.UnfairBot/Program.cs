﻿using System;
using System.Linq;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;

namespace SpurRoguelike.UnfairBot {
    public class UnfairBot : IPlayerController {
        public Turn MakeTurn(LevelView levelView, IMessageReporter messageReporter) {
            messageReporter.ReportMessage("Hey ho! I'm still breathing");

            if(levelView.Random.NextDouble() < 0.1)
                return Turn.None;

            var nearbyMonster = levelView.Monsters.FirstOrDefault(m => IsInAttackRange(levelView.Player.Location, m.Location));

            if(nearbyMonster.HasValue)
                return Turn.Attack(nearbyMonster.Location - levelView.Player.Location);

            return Turn.Step((StepDirection)levelView.Random.Next(4));
        }

        private static Boolean IsInAttackRange(Location a, Location b) => a.IsInRange(b, 1);
    }
}