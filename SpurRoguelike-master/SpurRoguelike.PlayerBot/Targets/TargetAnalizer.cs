using SpurRoguelike.PlayerBot.Game;
using SpurRoguelike.PlayerBot.Targets;

internal sealed class TargetAnalizer {
    private ITargetState<BaseTarget> currentState;

    public TargetAnalizer(GameMap gameMap) {
        PlayerGameInfo targets = new PlayerGameInfo {
            GameMap = gameMap,
            TargetMovementToExitOnOpenMap = new TargetMovementToExitOnOpenMap(gameMap),
            TargetTakeBestItemOnOpenMap = new TargetTakeBestItemOnOpenMap(gameMap),
            TargetOpenMap = new TargetOpenMap(gameMap),
            TargetMovementToExit = new TargetMovementToExit(gameMap),
            TargetTakeBestItem = new TargetTakeBestItem(gameMap),
            TargetTakeHealthPack = new TargetTakeHealthPack(gameMap),
            TargetAttackTheEnemy = new TargetAttackTheEnemy(gameMap),
            TargetMovementOnTheMonster = new TargetMovementOnTheMonster(gameMap),
            TargetMovementOnPosition = new TargetMovementOnPosition(gameMap)
        };
        currentState = new StateMovementOnTheMonster(targets.TargetMovementOnTheMonster, targets);
    }

    public TurnInfo GetTurn() {
        currentState = currentState.NextState();
        return currentState.CurrentTurn;
    }
}