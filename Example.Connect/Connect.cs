using System;
using System.Threading.Tasks;

namespace MbientLab.Warble.Example {
    class Connect {
        static void Main(string[] args) {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            var gatt = new Gatt(args[0]);

            await gatt.ConnectAsync();
            Console.WriteLine("Connected");

            await Task.Delay(5000);
            gatt.Disconnect();
        }
    }
}
