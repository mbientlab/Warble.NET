using static MbientLab.Warble.Bindings;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MbientLab.Warble {
    /// <summary>
    /// Wrapper class around the WarbleGattChar C struct
    /// </summary>
    public class GattChar {
        /// <summary>
        /// 128-bit UUID string identifying this GATT characteristic
        /// </summary>
        public String Uuid { get => Marshal.PtrToStringAnsi(warble_gattchar_get_uuid(WarbleGattChar)); }
        /// <summary>
        /// Handler to process characteristic notifications, <see cref="EnableNotificationsAsync"/>
        /// </summary>
        public Action<byte[]> OnNotificationReceived { get; set; }

        private readonly IntPtr WarbleGattChar;
        private readonly FnVoid_VoidP_WarbleGattCharP_UbyteP_Ubyte WarbleNotifyHandler;

        internal GattChar(IntPtr pointer) {
            WarbleGattChar = pointer;

            WarbleNotifyHandler = new FnVoid_VoidP_WarbleGattCharP_UbyteP_Ubyte((context, caller, value, value_size) => {
                if (OnNotificationReceived != null) {
                    byte[] managedArray = new byte[value_size];
                    Marshal.Copy(value, managedArray, 0, value_size);
                    OnNotificationReceived(managedArray);
                }
            });
            warble_gattchar_on_notification_received(WarbleGattChar, IntPtr.Zero, WarbleNotifyHandler);
        }

        private async Task WriteAsync(Action<FnVoid_VoidP_WarbleGattCharP_CharP> fn) {
            TaskCompletionSource<bool> warbleTaskSrc = new TaskCompletionSource<bool>();
            var completed = new FnVoid_VoidP_WarbleGattCharP_CharP((ctx, caller, err) => {
                if (err != null) {
                    warbleTaskSrc.SetException(new WarbleException(err));
                } else {
                    warbleTaskSrc.SetResult(true);
                }
            });

            fn(completed);

            await warbleTaskSrc.Task;
        }
        /// <summary>
        /// Writes value to the characteristic requiring an acknowledge from the remote device
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <returns>Null when the write operation completes</returns>
        /// <exception cref="WarbleException">If write operation fails</exception>
        public Task WriteAsync(byte[] value) {
            return WriteAsync(completed => warble_gattchar_write_async(WarbleGattChar, value, (byte)value.Length, IntPtr.Zero, completed));
        }
        /// <summary>
        /// Writes value to the characteristic without requesting a response from the remove device
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <returns>Null when the write operation completes</returns>
        /// <exception cref="WarbleException">If write operation fails</exception>
        public Task WriteWithoutResponseAsync(byte[] value) {
            return WriteAsync(completed => warble_gattchar_write_without_resp_async(WarbleGattChar, value, (byte)value.Length, IntPtr.Zero, completed));
        }

        /// <summary>
        /// Reads current value from the characteristic
        /// </summary>
        /// <returns>Characteristic value when the read operation completes</returns>
        /// <exception cref="WarbleException">If read operation fails</exception>
        public Task<byte[]> ReadAsync() {
            TaskCompletionSource<byte[]> warbleTaskSrc = new TaskCompletionSource<byte[]>();
            var completed = new FnVoid_VoidP_WarbleGattCharP_UbyteP_Ubyte_CharP((ctx, caller, value, value_size, err) => {
                if (err != null) {
                    warbleTaskSrc.SetException(new WarbleException(err));
                } else {
                    byte[] managedArray = new byte[value_size];
                    Marshal.Copy(value, managedArray, 0, value_size);
                    warbleTaskSrc.SetResult(managedArray);
                }
            });
            warble_gattchar_read_async(WarbleGattChar, IntPtr.Zero, completed);

            return warbleTaskSrc.Task;
        }

        private async Task EditNotification(Action<FnVoid_VoidP_WarbleGattCharP_CharP> fn) {
            TaskCompletionSource<bool> warbleTaskSrc = new TaskCompletionSource<bool>();
            var completed = new FnVoid_VoidP_WarbleGattCharP_CharP((ctx, caller, err) => {
                if (err != null) {
                    warbleTaskSrc.SetException(new WarbleException(err));
                } else {
                    warbleTaskSrc.SetResult(true);
                }
            });

            fn(completed);

            await warbleTaskSrc.Task;
        }
        /// <summary>
        /// Enables characteristic notifications, which are forwarded to the <see cref="OnNotificationReceived"/> delegate
        /// </summary>
        /// <returns>Null when notifications are enabled</returns>
        /// <exception cref="WarbleException">If notify enable operation fails</exception>
        public Task EnableNotificationsAsync() {
            return EditNotification(completed => warble_gattchar_enable_notifications_async(WarbleGattChar, IntPtr.Zero, completed));
        }
        /// <summary>
        /// Disables characteristic notifications 
        /// </summary>
        /// <returns>Null when notifications are disabled</returns>
        /// <exception cref="WarbleException">If notify disable operation fails</exception>
        public Task DisableNotificationsAsync() {
            return EditNotification(completed => warble_gattchar_disable_notifications_async(WarbleGattChar, IntPtr.Zero, completed));
        }

        [Obsolete("Deprecated in v1.0.4, use EnableNotificationsAsync instead")]
        public Task EnableNotifications() {
            return EnableNotificationsAsync();
        }
        [Obsolete("Deprecated in v1.0.4, use DisableNotificationsAsync instead")]
        public Task DisableNotifications() {
            return DisableNotificationsAsync();
        }
    }
}
