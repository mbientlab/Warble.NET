using System;
using System.Threading.Tasks;

namespace MbientLab.Warble.Example {
    class Program {
        static void Main(string[] args) {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            var gatt = new Gatt(args[0]);
            await gatt.ConnectAsync();

            string[] uuids = new string[] {
                "00002a26-0000-1000-8000-00805f9b34fb",
                "00002a24-0000-1000-8000-00805f9b34fb",
                "00002a27-0000-1000-8000-00805f9b34fb",
                "00002a29-0000-1000-8000-00805f9b34fb",
                "00002a25-0000-1000-8000-00805f9b34fb"
            };

            foreach(var id in uuids) {
                var gattchar = gatt.FindCharacteristic(id);                
                if (gattchar == null) {
                    Console.Write(id);
                    Console.WriteLine(": Does not exist");
                } else {
                    Console.Write(gattchar.Uuid);
                    Console.WriteLine(string.Format(": {0}", System.Text.Encoding.ASCII.GetString(await gattchar.ReadAsync())));
                }
            }
        }
    }
}
