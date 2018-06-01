using SpurRoguelike.WebPlayerBot.Game;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot {
    public interface IWebPlayerController {
        TurnType MakeTurn(LevelViewInfo levelView);
    }
}
