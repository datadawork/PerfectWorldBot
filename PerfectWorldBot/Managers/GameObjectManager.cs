using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using PerfectWorldBot.Enums;
using PerfectWorldBot.Objects;

namespace PerfectWorldBot.Managers {
    public static class GameObjectManager {
        //static GameObjectManager() { }
        private static HostPlayer _hostPlayer;

        private static ConcurrentDictionary<uint, GameObject> _cachedObjects =
            new ConcurrentDictionary<uint, GameObject>();

        public static HostPlayer HostPlayer {
            get {
                if (_hostPlayer != null) return _hostPlayer;
                _hostPlayer = new HostPlayer(Core.Offsets.Objects.HostPlayer);
                if (_hostPlayer.IsValid) return _hostPlayer;
                _hostPlayer = null;
                return _hostPlayer;
            }
        }

        public static IEnumerable<GameObject> GameObjects => _cachedObjects.Values as IEnumerable<GameObject>;

        private static IEnumerable<GameObject> GetRawObjects() {
            yield return HostPlayer;
            var epList = Core.Memory.ReadEx<IntPtr>(Core.Offsets.Objects.ElsePlayerListPtr);
            var epCount = Core.Memory.ReadEx<int>(Core.Offsets.Objects.ElsePlayerListCountPtr);
            for (var i = 0; i < epCount; i++) {
                var ptr = Core.Memory.ReadEx<IntPtr>(epList + i*4);
                var type = (GameObjectType) Core.Memory.ReadEx<int>(ptr + 0xb4);
                if (type == GameObjectType.ElsePlayer)
                    yield return new ElsePlayer(ptr);
                else if (type == GameObjectType.HostPlayer)
                    yield return new HostPlayer(ptr);
            }
            var npcList = Core.Memory.ReadEx<IntPtr>(Core.Offsets.Objects.NPCListPtr);
            var npcCount = Core.Memory.ReadEx<int>(Core.Offsets.Objects.NPCListCountPtr);
            for (var i = 0; i < npcCount; i++) {
                var ptr = Core.Memory.ReadEx<IntPtr>(npcList + i*4);
                var type = (GameObjectType) Core.Memory.ReadEx<int>(ptr + 0xb4);
                if (type == GameObjectType.NpcServer)
                    yield return new NpcServer(ptr);
                else if (type == GameObjectType.Monster)
                    yield return new Monster(ptr);
            }
        }

        public static GameObject GetObjectByObjectId(uint ObjectId) {
            GameObject gameObject;
            _cachedObjects.TryGetValue(ObjectId, out gameObject);
            return gameObject;
        }

        public static T GetObjectById<T>(uint id) where T : GameObject {
            return GameObjects.FirstOrDefault(o => o.ObjectId == id) as T;
        }

        public static IEnumerable<T> GetObjectsByType<T>() where T : GameObject {
            return
                GameObjects.Where(o => (o.GetType() == typeof(T)) || (o.GetType().BaseType == typeof(T)))
                    .Select(o => o as T);
        }

        public static void Update() {
            foreach (var gObject in GetRawObjects()) {
                if ((gObject == null) || !gObject.IsValid) continue;
                var id = gObject.ObjectId;
                if (_cachedObjects.ContainsKey(id)) {
                    // noting
                } else {
                    if (_cachedObjects.TryAdd(id, gObject)) { /*Logging.Log($"Add: {gObject}");*/
                    }
                }
            }
            foreach (var obj in _cachedObjects) {
                var gameObjectTmp = obj.Value;
                if ((gameObjectTmp == null) || gameObjectTmp.IsValid) continue;
                if (_cachedObjects.TryRemove(obj.Key, out gameObjectTmp)) { /*Logging.Log($"Remove: {gameObjectTmp}");*/
                }
            }
        }
    }
}