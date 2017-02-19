using System;
using System.Linq;

namespace PerfectWorldBot.Managers {
    public static class PacketManager {

        public static void ClearTarget() {
            const string packetStr = "0800";
            SendPacket(packetStr);
        }

        public static void SelectTarget(uint targetId) {
            targetId = ReverseBytes(targetId);
            var packetStr = $"0200{targetId:X8}";
            SendPacket(packetStr);
        }

        public static void UseSkillOnTargetId(uint uSkillId, uint uTargetId) {
            uSkillId = ReverseBytes(uSkillId);
            uTargetId = ReverseBytes(uTargetId);
            var packetStr = $"2900{uSkillId:X8}0001{uTargetId:X8}";
            SendPacket(packetStr);
        }

        internal static void SendPacket(string packetDataStr) {
            var packetData = StringToByteArray(packetDataStr);
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

        private static uint ReverseBytes(uint val) {
            var uintBytes = BitConverter.GetBytes(val);
            Array.Reverse(uintBytes);
            return BitConverter.ToUInt32(uintBytes, 0);
        }

        private static byte[] StringToByteArray(string hex) {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

    }
}