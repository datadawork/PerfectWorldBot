using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Binarysharp.MemoryManagement;
using PerfectWorldBot.Managers;
using PerfectWorldBot.Objects;

namespace PerfectWorldBot {
    public static class Core {
        private static BackgroundWorker bwBot = new BackgroundWorker();
        internal static MemorySharp Memory { get; private set; }
        internal static Offsets.Offsets Offsets { get; private set; }

        public static HostPlayer Me => GameObjectManager.HostPlayer;
        public static HostPlayer HostPlayer => GameObjectManager.HostPlayer;

        public static bool IsRunning { get; private set; }
        public static bool IsInGame => HostPlayer != null;
        public static event EventHandler<bool> BotStatusChanged;

        public static bool Init() {
            var procs = Process.GetProcessesByName("elementclient");
            if (procs.Length <= 0) {
                Logging.Log("[CORE] Game Process not found.");
                return false;
            }
            Memory = new MemorySharp(procs[0]);
            if (Memory == null) {
                Logging.Log("[CORE] Unable to init MemoryLib.");
                return false;
            }

            Offsets = new Offsets.Offsets();

            bwBot.WorkerSupportsCancellation = true;
            bwBot.DoWork += bwBotDoWork;
            bwBot.RunWorkerAsync();
            return true;
        }

        private static void bwBotDoWork(object sender, DoWorkEventArgs doWorkEventArgs) {
            var bw = sender as BackgroundWorker;
            if (bw == null) return;
            IsRunning = true;
            BotStatusChanged?.Invoke(sender, true);
            Logging.Log($"[CORE] Bot Started");
            // main loop
            while (!bw.CancellationPending) {
                Thread.Sleep(25);
            }
            IsRunning = false;
            BotStatusChanged?.Invoke(sender, false);
            Logging.Log($"[CORE] Bot Stopped");
        }

        public static void Start() {
            bwBot.RunWorkerAsync();
        }

        public static void Stop() {
            bwBot.CancelAsync();
        }
    }
}