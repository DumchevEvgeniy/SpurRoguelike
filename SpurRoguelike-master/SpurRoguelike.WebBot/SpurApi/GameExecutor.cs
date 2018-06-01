using System.Threading.Tasks;
using SpurRoguelike.WebPlayerBot.Infractructure;

namespace SpurRoguelike.WebPlayerBot {
    public class GameExecutor {
        private readonly ApiSpurClient spurClient = new ApiSpurClient();
        private readonly IWebPlayerController webPlayerController;
        private LevelViewInfo levelViewInfo;

        public GameExecutor(IWebPlayerController webPlayerController) {
            this.webPlayerController = webPlayerController;
        }

        public async Task Run() {
            await Start();
            while (true)
                await NextTurn();
        }

        private async Task NextTurn() {
            var apiActionStr = webPlayerController.MakeTurn(levelViewInfo).ToApi();
            levelViewInfo = await spurClient.ExecutePostAsync(apiActionStr);
        }

        private async Task Start() {
            levelViewInfo = await spurClient.ExecutePostAsync(OpenApiInfo.Start);
        }
    }
}
