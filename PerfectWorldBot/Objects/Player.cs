using System;
using PerfectWorldBot.Enums;

namespace PerfectWorldBot.Objects {
    public class Player : GameObject {
        public Player(IntPtr pointer) : base(pointer) {}

        public override uint CurrentHealth => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Player.HP);
        public override uint MaxHealth => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Player.HP_Max);
        public override float CurrentHealthPercent => (float) CurrentHealth/MaxHealth*100;

        public uint CurrentMana => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Player.MP);
        public uint MaxMana => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Player.MP_Max);
        public float CurrentManaPercent => (float) CurrentMana/MaxMana*100;

        public override float X => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Player.X);
        public override float Y => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Player.Y);
        public override float Z => Core.Memory.ReadEx<float>(Pointer + Core.Offsets.Player.Z);

        public override uint CurrentTargetId => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Player.TargetID);

        public override uint Level => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Player.Level);

        public virtual PlayerClass Class => (PlayerClass) Core.Memory.ReadEx<byte>(Pointer + Core.Offsets.Player.ClassID);
    }
}