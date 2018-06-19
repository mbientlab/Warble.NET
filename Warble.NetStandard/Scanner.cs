using static MbientLab.Warble.Bindings;
using System;
using System.Runtime.InteropServices;

namespace MbientLab.Warble {
    /// <summary>
    /// Information received from a discovered BLE device
    /// </summary>
    public class ScanResult {
        /// <summary>
        /// Mac address of the scanned device
        /// </summary>
        public string Mac { get => MarshalledResult.mac; }
        /// <summary>
        /// Device's advertising name
        /// </summary>
        public string Name { get => MarshalledResult.name; }
        /// <summary>
        /// Device's current signal strength
        /// </summary>
        public int Rssi { get => MarshalledResult.rssi; }

        private readonly IntPtr Pointer;
        private readonly Bindings.ScanResult MarshalledResult;

        internal ScanResult(IntPtr pointer) {
            Pointer = pointer;
            MarshalledResult = Marshal.PtrToStructure<Bindings.ScanResult>(Pointer);
        }

        /// <summary>
        /// Checks if the BLE ad packet contains the requested service UUID
        /// </summary>
        /// <param name="uuid">128-bit UUID string to search for</param>
        /// <returns>True if the device is advertising with the uuid, false otherwise</returns>
        public bool HasServiceUuid(string uuid) {
            return warble_scan_result_has_service_uuid(Pointer, uuid) != 0;
        }
        /// <summary>
        /// Additional data from the manufacturer included in the scan response
        /// </summary>
        /// <param name="companyId">Unsigned short value to look up</param>
        /// <returns>Manufacturer data, null if company ID is not found</returns>
        public byte[] GetManufacturerData(ushort companyId) {
            IntPtr data = warble_scan_result_get_manufacturer_data(Pointer, companyId);
            if (data == IntPtr.Zero) {
                return null;
            }

            var marshalled = Marshal.PtrToStructure<ScanMftData>(data);
            byte[] managedArray = new byte[marshalled.value_size];
            Marshal.Copy(marshalled.value, managedArray, 0, marshalled.value_size);

            return managedArray;
        }
    }
    /// <summary>
    /// Controls BLE scanning
    /// </summary>
    public class Scanner {
        /// <summary>
        /// Handler to listen for BLE scan results
        /// </summary>
        public static Action<ScanResult> OnResultReceived { get; set; }
        private static readonly FnVoid_VoidP_WarbleScanResultP WarbleResultReceived;

        static Scanner() {
            WarbleResultReceived = (ctx, pointer) => OnResultReceived?.Invoke(new ScanResult(pointer));
            warble_scanner_set_handler(IntPtr.Zero, WarbleResultReceived);
        }

        /// <summary>
        /// Start BLE scanning
        /// </summary>
        /// <param name="hci"></param>
        public static void Start(string hci = null) {
            if (hci != null && RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                Option[] options = new Option[1] {
                    new Option {
                        key = "hci",
                        value = hci
                    }
                };
                warble_scanner_start(1, options);
            } else {
                warble_scanner_start(0, null);
            }
        }
        /// <summary>
        /// Stop BLE scanning
        /// </summary>
        public static void Stop() {
            warble_scanner_stop();
        }
    }
}
