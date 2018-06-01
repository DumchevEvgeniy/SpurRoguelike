using System;
using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot {
    public sealed class WebPlayerBot : IWebPlayerController {
        private GameMap gameMap;
        private TargetAnalizer targetAnalizer;
        private Int32 maxPlayerHealth = 0;

        public TurnType MakeTurn(LevelViewInfo levelViewInfo) {
            if (gameMap == null) {
                maxPlayerHealth = Math.Max(maxPlayerHealth, levelViewInfo.LevelData.Player.Health);
                gameMap = new GameMap(levelViewInfo.Field, levelViewInfo.LevelData.Player, maxPlayerHealth);
                targetAnalizer = new TargetAnalizer(gameMap);
            }
            gameMap.UpdateState(levelViewInfo);
            var turnInfo = targetAnalizer.GetTurn();
            if (turnInfo == null || turnInfo.Turn == null)
                return default(TurnType);
            if (IsLastStep(turnInfo.TurnType))
                gameMap = null;
            return turnInfo.TurnType;
        }

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
