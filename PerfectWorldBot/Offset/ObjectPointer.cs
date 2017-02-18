using System;

namespace PerfectWorldBot.Offset {
    internal class ObjectPointer {
        internal readonly int Base = 0x00DA433C;

        internal IntPtr Game => Core.Memory.ReadEx<IntPtr>((IntPtr) Base);
        internal IntPtr GameRun => Core.Memory.ReadEx<IntPtr>(Game + 0x1C);
        internal IntPtr World => Core.Memory.ReadEx<IntPtr>(GameRun + 0x14);
        internal IntPtr HostPlayer => Core.Memory.ReadEx<IntPtr>(GameRun + 0x28);
        internal IntPtr PlayerMan => Core.Memory.ReadEx<IntPtr>(World + 0x1C);
        internal IntPtr NPCMan => Core.Memory.ReadEx<IntPtr>(World + 0x20);

        internal IntPtr ElsePlayerListPtr => PlayerMan + 0x98;
        internal IntPtr ElsePlayerListCountPtr => PlayerMan + 0x9C;
        internal IntPtr NPCListPtr => NPCMan + 0x5C;
        internal IntPtr NPCListCountPtr => NPCMan + 0x60;
    }
}