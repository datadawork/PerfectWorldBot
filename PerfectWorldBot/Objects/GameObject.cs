using System;
using System.Numerics;
using System.Text;
using PerfectWorldBot.Enums;
using PerfectWorldBot.Managers;

namespace PerfectWorldBot.Objects {
    public class GameObject : RemoteObject {
        internal GameObject(IntPtr pointer) : base(pointer) {}

        public uint ObjectId {
            get {
                switch (Type) {
                    case GameObjectType.HostPlayer:
                        return Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.HostPlayer.ID);
                    case GameObjectType.ElsePlayer:
                        return Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.ElsePlayer.ID);
                    case GameObjectType.Monster:
                        return Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Monster.ID);
                    case GameObjectType.NpcServer:
                        return Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.NpcServer.ID);
                    default:
                        return 0u;
                }
            }
        }

        public GameObjectType Type => (GameObjectType) Core.Memory.ReadEx<int>(Pointer + 0xb4);

        public string Name {
            get {
                var p = IntPtr.Zero;
                switch (Type) {
                    case GameObjectType.HostPlayer:
                        p = Core.Memory.ReadEx<IntPtr>(Pointer + Core.Offsets.HostPlayer.NamePtr);
                        break;
                    case GameObjectType.ElsePlayer:
                        p = Core.Memory.ReadEx<IntPtr>(Pointer + Core.Offsets.ElsePlayer.NamePtr);
                        break;
                    case GameObjectType.Monster:
                        p = Core.Memory.ReadEx<IntPtr>(Pointer + Core.Offsets.Monster.NamePtr);
                        break;
                    case GameObjectType.NpcServer:
                        p = Core.Memory.ReadEx<IntPtr>(Pointer + Core.Offsets.NpcServer.NamePtr);
                        break;
                }
                return p == IntPtr.Zero ? null : Core.Memory.ReadString(p, Encoding.Unicode, false);
            }
        }

        public override bool IsValid {
            get {
                if (Pointer == IntPtr.Zero) return false;
                if (!IsValidName(Name)) return false;
                return ObjectId > 0u;
            }
        }

        private bool IsValidName(string name) {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (Encoding.UTF8.GetByteCount(name) != name.Length) return false;
            if (Encoding.UTF8.GetByteCount(name) <= 1) return false;
            return true;
        }

        public override string ToString() => $"{Name} - {ObjectId:X8}";

        #region Virtual

        public virtual uint CurrentHealth => 0u;
        public virtual uint MaxHealth => 0u;
        public virtual float CurrentHealthPercent => 0.0f;
        public virtual float X => 0.0f;
        public virtual float Y => 0.0f;
        public virtual float Z => 0.0f;
        public virtual Vector3 CurrentPosition => new Vector3(X, Y, Z);
        public virtual uint Level => 0u;
        public virtual uint CurrentTargetId => 0u;
        public virtual bool HasTarget => CurrentTargetId != 0u;
        public virtual GameObject CurrentTarget => GameObjectManager.GetObjectById(CurrentTargetId);

        #endregion

        #region EqualityCompare

        protected bool Equals(GameObject other) => ObjectId == other.ObjectId;

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GameObject) obj);
        }

        public override int GetHashCode() => (int) ObjectId;

        public static bool operator ==(GameObject a, GameObject b) {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (!ReferenceEquals(a, null) && !ReferenceEquals(b, null) && !a.Equals(b)) return false;
            if (!ReferenceEquals(a, null) && !ReferenceEquals(b, null) && a.Equals(b)) return true;
            return false;
        }

        public static bool operator !=(GameObject a, GameObject b) => !(a == b);

        #endregion
    }
}