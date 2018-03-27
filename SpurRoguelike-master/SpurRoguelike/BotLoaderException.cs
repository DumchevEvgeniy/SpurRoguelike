using System;

namespace SpurRoguelike {
    internal class BotLoaderException : Exception {
        public BotLoaderException(String message)
            : base(message) {
        }

        public BotLoaderException(String message, Exception innerException)
            : base(message, innerException) {
        }
    }
}