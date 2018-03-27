using System;

namespace SpurRoguelike.ConsoleGUI.Panels {
    internal abstract class CenteredPanel : Panel {
        protected CenteredPanel(ITextScreen screen, Int32 blankSize, Int32 contentWidth, Int32 contentHeight)
            : base(GetDisposition(screen.Width, screen.Height, blankSize, contentWidth, contentHeight), screen) {
        }

        public override void RedrawBorder() {
        }

        private static ScreenZone GetDisposition(Int32 screenWidth, Int32 screenHeight, Int32 blankSize, Int32 contentWidth, Int32 contentHeight) {
            var width = 2 + blankSize * 2 + contentWidth;
            var height = 2 + blankSize * 2 + contentHeight;

            return new ScreenZone(Math.Max(0, screenWidth / 2 - width / 2), Math.Max(0, screenHeight / 2 - height / 2), width, height);
        }
    }
}