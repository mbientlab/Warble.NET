using System.Runtime.InteropServices;
using static MbientLab.Warble.Bindings;

namespace MbientLab.Warble {
    /// <summary>
    /// General library level functions
    /// </summary>
    public class Library {
        /// <summary>
        /// Checks the version of the native Warble C library used by the wrwapper
        /// </summary>
        /// <returns>Semantic version in the form x.y.z</returns>
        public static string GetVersion() {
            return Marshal.PtrToStringAnsi(warble_lib_version());
        }
        /// <summary>
        /// Checks the build configuration of the native Warble C library used by the wrapper
        /// </summary>
        /// <returns>Either 'Release' or 'Debug'</returns>
        public static string GetConfig() {
            return Marshal.PtrToStringAnsi(warble_lib_config());
        }

        /// <summary>
        /// Initializes the Warble library
        /// </summary>
        /// <param name="logLevel">One of: 'error', 'warning', 'info', 'debug', 'trace', only available on Linux</param>
        public static void Init(string logLevel = null) {
            if (logLevel != null && RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                Option[] options = new Option[1] {
                    new Option {
                        key = "log-level",
                        value = logLevel
                    }
                };
                warble_lib_init(1, options);
            } else {
                warble_lib_init(0, null);
            }
        }
    }
}
