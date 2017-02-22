using System;

namespace PerfectWorldBot.Objects {
    public class Matter: GameObject {
        public Matter(IntPtr pointer) : base(pointer) {}

        public override float X => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Matter.X);
        public override float Y => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Matter.Y);
        public override float Z => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Matter.Z);

        public uint ItemId => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Matter.ItemId);
        public uint TypeId => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Matter.Type);
        public bool IsItem => TypeId != 2;
        public float Distance => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Matter.Distance);
    }
}