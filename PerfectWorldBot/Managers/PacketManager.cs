using System;

namespace PerfectWorldBot.Managers {
    public static class PacketManager {

        public static void ClearTarget()
        {
            var packet = new byte[] { 0x08, 0x00 };
            SendPacket(packet);
        }

        internal static void SendPacket(byte[] packetData) {
            using (var packetPtr = Core.Memory.Memory.Allocate(packetData.Length)) {
                packetPtr.Write(packetData);
                using (var asm = Core.Memory.Assembly.BeginTransaction()) {
                    asm.AddLine("pushad");
                    asm.AddLine("mov eax, {0}", Core.Offsets.Functions.SendPacket);
                    asm.AddLine("mov ecx, [{0}]", Core.Offsets.Objects.Base);
                    asm.AddLine("mov ecx, [ecx+0x20]");
                    asm.AddLine("mov edi, {0}", packetPtr.BaseAddress.ToInt32());
                    asm.AddLine("push {0}", packetData.Length);
                    asm.AddLine("push edi");
                    asm.AddLine("call eax");
                    asm.AddLine("popad");
                    asm.AddLine("retn");
                }
            }
        }
    }
}