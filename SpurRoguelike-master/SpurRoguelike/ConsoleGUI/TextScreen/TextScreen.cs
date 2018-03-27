using System;
using System.Runtime.InteropServices;
using SpurRoguelike.ConsoleGUI.WinApi;

namespace SpurRoguelike.ConsoleGUI.TextScreen {
    internal class TextScreen : ITextScreen {
        public TextScreen(Int32 width, Int32 height) {
            OpenConsoleHandle();

            Console.WindowWidth = width;
            Console.WindowHeight = height;

            SetupWindow();
        }

        public TextScreen() {
            OpenConsoleHandle();

            ShowWindow(GetConsoleWindow(), 3);

            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WindowHeight = Console.LargestWindowHeight;

            SetupWindow();
        }

        public void Clear() {
            Fill(new ScreenZone(0, 0, Width, Height), ConsoleCharacter.Empty);
            Render();
        }

        public void Put(Int32 left, Int32 top, ConsoleCharacter character) {
            if(left < 0 || left >= Width || top < 0 || top >= Height)
                return;

            frameBuffer[top, left] = new CharInfo(character.Character, CharAttributeConverter.MakeCharAttributes(character.Color, character.BackgroundColor));
        }

        public void Fill(ScreenZone zone, ConsoleCharacter character) {
            for(Int32 i = 0; i < zone.Width; i++) {
                for(Int32 j = 0; j < zone.Height; j++) {
                    Put(zone.Left + i, zone.Top + j, character);
                }
            }
        }

        public void Write(Int32 left, Int32 top, ConsoleMessage message) {
            Write(left, top, message, 0);
        }

        public void Write(Int32 left, Int32 top, ConsoleMessage message, Int32 redrawLength) {
            var textToDraw = message.Text;
            if(redrawLength > 0)
                textToDraw = textToDraw.PadRight(redrawLength);

            for(Int32 i = 0; i < textToDraw.Length; i++)
                Put(left + i, top, new ConsoleCharacter(textToDraw[i], message.Color, message.BackgroundColor));
        }

        public void Render() {
            var consoleRect = new SmallRect(0, 0, (Int16)Width, (Int16)Height);
            WriteConsoleOutput(consoleHandle, frameBuffer, new Coord((Int16)Width, (Int16)Height), new Coord(0, 0), ref consoleRect);
        }

        public void Render(ScreenZone zone) {
            var consoleRect = new SmallRect((Int16)zone.Left, (Int16)zone.Top, (Int16)(zone.Width + zone.Left - 1), (Int16)(zone.Height + zone.Top - 1));
            WriteConsoleOutput(consoleHandle, frameBuffer, new Coord((Int16)Width, (Int16)Height), new Coord((Int16)zone.Left, (Int16)zone.Top), ref consoleRect);
        }

        public Int32 Width { get; private set; }

        public Int32 Height { get; private set; }

        private void OpenConsoleHandle() {
            consoleHandle = CreateFile("CONOUT$", 0x40000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
        }

        private void SetupWindow() {
            Console.Clear();

            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);

            Width = Console.WindowWidth;
            Height = Console.WindowHeight;

            frameBuffer = new CharInfo[Height, Width];

            Clear();
        }

        private CharInfo[,] frameBuffer;
        private IntPtr consoleHandle;

        #region WinApi imports

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateFile(String fileName, Int32 desiredAccess, Int32 shareMode, IntPtr securityAttributes, Int32 creationDisposition, Int32 flagsAndAttributes, IntPtr templateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern Boolean WriteConsoleOutput(IntPtr consoleHandle, CharInfo[,] charBuffer, Coord bufferSize, Coord bufferOffset, ref SmallRect writeRegion);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr windowHandle, Int32 command);

        #endregion
    }
}