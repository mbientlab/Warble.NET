using System;
using System.Threading.Tasks;

namespace MbientLab.Warble.Example {
    class LeScan {
        static void Main(string[] args) {
            Console.WriteLine("version: " + Library.GetVersion());
            Console.WriteLine("config: " + Library.GetConfig());
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args) {
            Scanner.OnResultReceived = result => {
                Console.WriteLine("mac: " + result.Mac);
                Console.WriteLine("name: " + result.Name);
                Console.WriteLine(string.Format("rssi: {0}dBm", result.Rssi));

                Console.WriteLine("metawear service? " + result.HasServiceUuid("326a9000-85cb-9195-d9dd-464cfbbae75a"));

                Console.Write("mbientlab manufacturer data? ");
                var data = result.GetManufacturerData(0x626d);
                if (data != null) {
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("    value: [0x{0}]", BitConverter.ToString(data).ToLower().Replace("-", ", 0x")));
                } else {
                    Console.WriteLine(" false");
                }

                Console.WriteLine("======");
            };

            Console.WriteLine("-- active scan --");
            Scanner.Start();
            await Task.Delay(5000);
            Scanner.Stop();

            Console.WriteLine("-- passive scan --");
            Scanner.Start(scanType: "passive");
            await Task.Delay(5000);
            Scanner.Stop();
        }
    }
}
