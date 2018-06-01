using SpurRoguelike.PlayerBot.Game;
using SpurRoguelike.WebBot.Infractructure;

namespace SpurRoguelike.WebBot {
    public interface IWebPlayerController {
        TurnType MakeTurn(LevelViewInfo levelView);
    }
}
