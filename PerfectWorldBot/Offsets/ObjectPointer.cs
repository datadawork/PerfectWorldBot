using System;

namespace PerfectWorldBot.Offsets {
    public class ObjectPointer {
        public readonly int Base = 0x00DA433C;

        public int ObjectType = 0xb4;

        public IntPtr Game => Core.Memory.ReadEx<IntPtr>((IntPtr) Base);
        public IntPtr GameRun => Core.Memory.ReadEx<IntPtr>(Game + 0x1C);
        public IntPtr World => Core.Memory.ReadEx<IntPtr>(GameRun + 0x14);
        public IntPtr HostPlayer => Core.Memory.ReadEx<IntPtr>(GameRun + 0x28);
        public IntPtr PlayerMan => Core.Memory.ReadEx<IntPtr>(World + 0x1C);
        public IntPtr NPCMan => Core.Memory.ReadEx<IntPtr>(World + 0x20);

        public IntPtr ElsePlayerListPtr => PlayerMan + 0x98;
        public IntPtr ElsePlayerListCountPtr => PlayerMan + 0x9C;
        public IntPtr NPCListPtr => NPCMan + 0x5C;
        public IntPtr NPCListCountPtr => NPCMan + 0x60;
    }
}