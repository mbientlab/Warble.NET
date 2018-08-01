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
        /// <param name="hci">Mac address of the hci device to use, only applicable on Linux</param>
        /// <param name="scanType">Type of ble scan to perform, either 'passive' or 'active'</param>
        public static void Start(string hci = null, string scanType = null) {
            Option[] options = new Option[2];
            byte actualSize = 0;

            if (hci != null && RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                options[actualSize] = new Option {
                    key = "hci",
                    value = hci
                };
                actualSize++;
            }
            if (scanType != null) {
                options[actualSize] = new Option {
                    key = "scan-type",
                    value = scanType
                };
                actualSize++;
            }

            warble_scanner_start(actualSize, options);
        }
        /// <summary>
        /// Stop BLE scanning
        /// </summary>
        public static void Stop() {
            warble_scanner_stop();
        }
    }
}
