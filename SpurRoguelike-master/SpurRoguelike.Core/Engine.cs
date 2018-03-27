using System;
using SpurRoguelike.Core.Entities;

namespace SpurRoguelike.Core {
    public class Engine {
        public Engine(String playerName, IPlayerController playerController, Level entryLevel, IRenderer renderer, IEventReporter eventReporter) {
            this.playerName = playerName;
            this.playerController = playerController;
            this.entryLevel = entryLevel;
            this.renderer = renderer;
            this.eventReporter = eventReporter;
        }

        public void GameLoop() {
            var player = new Player(playerName, 10, 10, 100, 100, playerController, eventReporter);
            //var player = new Player(playerName, 38, 31, 100, 100, playerController, eventReporter);
            //var player = new Player(playerName, 100, 100, 1000, 1000, playerController, eventReporter);
            entryLevel.Spawn(entryLevel.Field.PlayerStart, player);

            while(!player.IsDestroyed) {
                renderer.RenderLevel(player.Level);
                player.Level.Tick();
            }

            renderer.RenderGameEnd(player.Level.IsCompleted);
        }

        private readonly String playerName;
        private readonly IPlayerController playerController;
        private readonly Level entryLevel;
        private readonly IRenderer renderer;
        private readonly IEventReporter eventReporter;
    }
}