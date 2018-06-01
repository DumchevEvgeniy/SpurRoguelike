using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Targets;

internal sealed class PlayerGameInfo {
    public GameMap GameMap { get; set; }

    public TargetMovementToExitOnOpenMap TargetMovementToExitOnOpenMap { get; set; }
    public TargetTakeBestItemOnOpenMap TargetTakeBestItemOnOpenMap { get; set; }
    public TargetOpenMap TargetOpenMap { get; set; }
    public TargetMovementToExit TargetMovementToExit { get; set; }
    public TargetTakeBestItem TargetTakeBestItem { get; set; }
    public TargetTakeHealthPack TargetTakeHealthPack { get; set; }
    public TargetAttackTheEnemy TargetAttackTheEnemy { get; set; }
    public TargetMovementOnTheMonster TargetMovementOnTheMonster { get; set; }
    public TargetMovementOnPosition TargetMovementOnPosition { get; set; }
}