using System;
using System.Runtime.InteropServices;

namespace MbientLab.Warble {
    sealed class Bindings {
        [StructLayout(LayoutKind.Sequential)]
        internal struct ScanMftData {
            public IntPtr value;
            public byte value_size;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct ScanResult {
            [MarshalAs(UnmanagedType.LPStr)]
            public string mac;
            [MarshalAs(UnmanagedType.LPStr)]
            public string name;
            public int rssi;
            public IntPtr private_data;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Option {
            [MarshalAs(UnmanagedType.LPStr)]
            public string key;
            [MarshalAs(UnmanagedType.LPStr)]
            public string value;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void FnVoid_IntPtr_WarbleGattP_CharP(IntPtr context, IntPtr gatt, [MarshalAs(UnmanagedType.LPStr)] string err);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void FnVoid_IntPtr_WarbleGattP_Int(IntPtr context, IntPtr gatt, int value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void FnVoid_VoidP_WarbleScanResultP(IntPtr context, IntPtr result);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void FnVoid_VoidP_WarbleGattCharP_CharP(IntPtr context, IntPtr gattchar, [MarshalAs(UnmanagedType.LPStr)] string err);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void FnVoid_VoidP_WarbleGattCharP_UbyteP_Ubyte_CharP(IntPtr context, IntPtr gattchar, IntPtr value, byte length, [MarshalAs(UnmanagedType.LPStr)] string err);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate void FnVoid_VoidP_WarbleGattCharP_UbyteP_Ubyte(IntPtr context, IntPtr gattchar, IntPtr value, byte length);

        private const string WARBLE_DLL = "warble";

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_lib_version();

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_lib_config();

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_lib_init(int length, Option[] options);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_scanner_stop();

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_scanner_start(int length, Option[] options);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_scanner_set_handler(IntPtr context, FnVoid_VoidP_WarbleScanResultP handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_scan_result_get_manufacturer_data(IntPtr result, ushort companyId);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int warble_scan_result_has_service_uuid(IntPtr result, [MarshalAs(UnmanagedType.LPStr)] string uuid);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gatt_connect_async(IntPtr gatt, IntPtr context, FnVoid_IntPtr_WarbleGattP_CharP handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gatt_disconnect(IntPtr gatt);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gatt_delete(IntPtr gatt);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gatt_on_disconnect(IntPtr gatt, IntPtr context, FnVoid_IntPtr_WarbleGattP_Int handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int warble_gatt_is_connected(IntPtr gatt);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_gatt_create([MarshalAs(UnmanagedType.LPStr)] string mac);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_gatt_create_with_options(int length, Option[] options);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_gatt_find_characteristic(IntPtr gatt, [MarshalAs(UnmanagedType.LPStr)] string uuid);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int warble_gatt_has_service(IntPtr gatt, [MarshalAs(UnmanagedType.LPStr)] string uuid);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gattchar_disable_notifications_async(IntPtr gattchar, IntPtr context, FnVoid_VoidP_WarbleGattCharP_CharP handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gattchar_write_without_resp_async(IntPtr gattchar, byte[] value, byte value_size, IntPtr context, FnVoid_VoidP_WarbleGattCharP_CharP handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gattchar_read_async(IntPtr gattchar, IntPtr context, FnVoid_VoidP_WarbleGattCharP_UbyteP_Ubyte_CharP handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gattchar_write_async(IntPtr gattchar, byte[] value, byte value_size, IntPtr context, FnVoid_VoidP_WarbleGattCharP_CharP handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gattchar_enable_notifications_async(IntPtr gattchar, IntPtr context, FnVoid_VoidP_WarbleGattCharP_CharP handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void warble_gattchar_on_notification_received(IntPtr gattchar, IntPtr context, FnVoid_VoidP_WarbleGattCharP_UbyteP_Ubyte handler);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_gattchar_get_uuid(IntPtr gattchar);

        [DllImport(WARBLE_DLL, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr warble_gattchar_get_gatt(IntPtr gattchar);

    }
}
