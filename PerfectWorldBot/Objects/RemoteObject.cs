using System;

namespace PerfectWorldBot.Objects {
    public class RemoteObject {
        internal RemoteObject(IntPtr pointer) {
            Pointer = pointer;
        }

        public IntPtr Pointer { get; private set; }

        public virtual bool IsValid => Pointer != IntPtr.Zero;
        internal IntPtr vTable => Core.Memory.Read<IntPtr>(Pointer);

        internal void _reBase(IntPtr pointer) => Pointer = pointer;
    }
}