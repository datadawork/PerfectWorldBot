using System;

namespace PerfectWorldBot.Objects {
    public class Npc : GameObject {
        public Npc(IntPtr pointer) : base(pointer) {}

        public override uint CurrentHealth => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Npc.HP);
        public override uint MaxHealth => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Npc.HP_Max);
        public override float CurrentHealthPercent => (float) CurrentHealth/MaxHealth*100;

        public override float X => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Npc.X);
        public override float Y => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Npc.Y);
        public override float Z => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Npc.Z);

        public override uint Level => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Npc.Level);
    }
}