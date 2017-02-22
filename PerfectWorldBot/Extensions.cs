using System;
using System.Linq;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Assembly;

namespace PerfectWorldBot {
    public static class Extensions {
        public static string ToHex(this IntPtr p) => $"{p.ToInt32():X8}";

        public static T ReadEx<T>(this MemorySharp _mem, IntPtr Address) => Read<T>(_mem, false, Address);

        public static T Read<T>(this MemorySharp _mem, params IntPtr[] addresses) => Read<T>(_mem, false, addresses);

        private static T Read<T>(this MemorySharp _mem, bool isRelative = false, params IntPtr[] addresses) {
            if (addresses.Length == 0)
                throw new InvalidOperationException("Cannot read a value from unspecified addresses.");

            if (addresses.Length == 1)
                return _mem.Read<T>(addresses[0], isRelative);

            var temp = _mem.Read<IntPtr>(addresses[0], isRelative);
            if (temp == IntPtr.Zero)
                throw new InvalidOperationException("First addresses returned 0");

            for (var i = 1; i < addresses.Length - 1; i++)
                temp = _mem.Read<IntPtr>(temp + (int) addresses[i], isRelative);
            return _mem.Read<T>(temp + (int) addresses[addresses.Length - 1], isRelative);
        }

        public static void AddLines(this AssemblyTransaction assemblyTransaction, object[] p) {
            for (var i = 0; i < p.Length; i++) {
                var s = p[i] as string;
                if (s == null)
                    continue;
                assemblyTransaction.AddLine(s, p.Skip(i + 1).ToArray());
            }
        }
    }
}