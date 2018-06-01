using System;
using System.Threading.Tasks;

namespace SpurRoguelike.WebBot {
    class Program {
        static void Main(string[] args) {
            var gameExecutor = new GameExecutor(new WebPlayerBot());
            var res = Task.Run(() => gameExecutor.Run()).ContinueWith(_ => Console.Write("close"));

            Console.ReadKey();
        }
    }
}
