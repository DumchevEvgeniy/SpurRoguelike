using System;
using SpurRoguelike.Core.Primitives;
using SpurRoguelike.PlayerBot.Game;
using SpurRoguelike.WebBot.Infractructure;

namespace SpurRoguelike.WebBot {
    public sealed class WebPlayerBot : IWebPlayerController {
        private GameMap gameMap;
        private TargetAnalizer targetAnalizer;
        private Int32 maxPlayerHealth = 0;

        public TurnType MakeTurn(LevelViewInfo levelView) {
            if (gameMap == null) {
                maxPlayerHealth = Math.Max(maxPlayerHealth, levelView.LevelData.Player.Health);
                gameMap = new GameMap(levelView.Field, levelView.Player, maxPlayerHealth);
                targetAnalizer = new TargetAnalizer(gameMap);
            }
            gameMap.UpdateState(levelView);
            var turnInfo = targetAnalizer.GetTurn();
            if (turnInfo == null || turnInfo.Turn == null)
                return DefaulTurn;
            if (IsLastStep(turnInfo.TurnType))
                gameMap = null;
            return turnInfo.Turn;
            return default(TurnType);
        }

        private Turn DefaulTurn => Turn.None;

        private Boolean IsLastStep(TurnType turnType) {
            var playerLoc = gameMap.AreaInfo.Player.Location;
            if (turnType == TurnType.StepToTheLeft && gameMap[playerLoc.X - 1, playerLoc.Y] == MapCellType.Exit)
                return true;
            if (turnType == TurnType.StepToTheRight && gameMap[playerLoc.X + 1, playerLoc.Y] == MapCellType.Exit)
                return true;
            if (turnType == TurnType.StepToTheTop && gameMap[playerLoc.X, playerLoc.Y - 1] == MapCellType.Exit)
                return true;
            if (turnType == TurnType.StepToTheBottom && gameMap[playerLoc.X, playerLoc.Y + 1] == MapCellType.Exit)
                return true;
            return false;
        }
    }
}
