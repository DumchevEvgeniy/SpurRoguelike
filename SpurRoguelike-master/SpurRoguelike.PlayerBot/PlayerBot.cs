using System;
using SpurRoguelike.Core;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.Core.Views;
using SpurRoguelike.PlayerBot.Game;

namespace SpurRoguelike.PlayerBot {
    public sealed class PlayerBot : IPlayerController {
        private GameMap gameMap;
        private TargetAnalizer targetAnalizer;
        private Int32 maxPlayerHealth = 0;
        private Int32 idLevel = 0;

        public Turn MakeTurn(LevelView levelView, IMessageReporter messageReporter) {
            if(gameMap == null) {
                maxPlayerHealth = Math.Max(maxPlayerHealth, levelView.Player.Health);
                gameMap = new GameMap(levelView.Field, levelView.Player, maxPlayerHealth);
                targetAnalizer = new TargetAnalizer(gameMap);
                idLevel++;
            }
            gameMap.UpdateState(levelView);
            var turnInfo = targetAnalizer.GetTurn();
            if(turnInfo == null || turnInfo.Turn == null)
                return DefaulTurn;
            if(IsLastStep(turnInfo.TurnType))
                gameMap = null;
            return turnInfo.Turn;
        }

        private Turn DefaulTurn => Turn.None;

        private Boolean IsLastStep(TurnType turnType) {
            var playerLoc = gameMap.AreaInfo.Player.Location;
            if(turnType == TurnType.StepToTheLeft && gameMap[playerLoc.X - 1, playerLoc.Y] == MapCellType.Exit)
                return true;
            if(turnType == TurnType.StepToTheRight && gameMap[playerLoc.X + 1, playerLoc.Y] == MapCellType.Exit)
                return true;
            if(turnType == TurnType.StepToTheTop && gameMap[playerLoc.X, playerLoc.Y - 1] == MapCellType.Exit)
                return true;
            if(turnType == TurnType.StepToTheBottom && gameMap[playerLoc.X, playerLoc.Y + 1] == MapCellType.Exit)
                return true;
            return false;
        }
    }
}