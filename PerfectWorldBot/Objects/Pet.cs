using System;

using PerfectWorldBot.Managers;

namespace PerfectWorldBot.Objects {
    public class Pet: Npc {
        public Pet(IntPtr pointer) : base(pointer) {}

        public uint OwnerId => Core.Memory.ReadEx<uint>(Pointer + Core.Offsets.Pet.OwnerId);
        public GameObject Owner => GameObjectManager.GetObjectById(OwnerId);

    }
}