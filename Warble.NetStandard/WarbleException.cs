using System;

namespace MbientLab.Warble {
    /// <summary>
    /// Represents errors that occur with the Warble library
    /// </summary>
    public class WarbleException : Exception {
        public WarbleException(String msg) : base(msg) {
        }
    }
}