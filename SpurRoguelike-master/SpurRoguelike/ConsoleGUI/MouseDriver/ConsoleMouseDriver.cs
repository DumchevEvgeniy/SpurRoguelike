using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SpurRoguelike.ConsoleGUI.WinApi;

namespace SpurRoguelike.ConsoleGUI.MouseDriver {
    internal class ConsoleMouseDriver {
        public ConsoleMouseDriver(IClickHandler clickHandler) {
            this.clickHandler = clickHandler;
            inputHandle = GetStdHandle(InputHandle);
        }

        public ConsoleKeyInfo WaitForInput() {
            while(true) {
                if(!GetNumberOfConsoleInputEvents(inputHandle, out var numberOfEvents))
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                if(numberOfEvents > 0) {
                    if(!ReadConsoleInput(inputHandle, out var record, 1, out numberOfEvents))
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    if(numberOfEvents == 0 || !Enum.IsDefined(typeof(EventType), record.EventType))
                        continue;

                    if(record.EventType == EventType.MouseEvent) {
                        MouseLeft = record.MouseEvent.MousePosition.X;
                        MouseTop = record.MouseEvent.MousePosition.Y;

                        if(record.MouseEvent.ButtonState.HasFlag(MouseButtonState.Leftmost))
                            clickHandler?.HandleMouseClick(MouseLeft, MouseTop);

                        continue;
                    }

                    if(record.KeyEvent.IsKeyDown == 0)
                        continue;

                    return ConsoleKeyConverter.ConvertEventToKey(record.KeyEvent);
                }
            }
        }

        public Int32 MouseLeft { get; private set; }

        public Int32 MouseTop { get; private set; }

        private readonly IntPtr inputHandle;

        private readonly IClickHandler clickHandler;

        #region WinApi imports

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(Int32 handleType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean GetNumberOfConsoleInputEvents(IntPtr consoleHandle, out Int32 numberOfEvents);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Boolean ReadConsoleInput(IntPtr consoleInputHandle, out InputRecord inputRecord, Int32 recordCount, out Int32 numberOfEventsRead);

        private const Int32 InputHandle = -10;

        #endregion
    }
}