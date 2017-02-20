using System;
using System.Windows.Forms;
using PerfectWorldBot.Managers;

namespace PerfectWorldBot {
    public partial class FormMainBot : Form {
        public FormMainBot() {
            InitializeComponent();
            Logging.SetLogControl(textBoxLog);
            Core.BotStatusChanged += CoreOnBotStatusChanged;
            textBoxBotStatus.Text = "Bot not Running";
        }

        private void CoreOnBotStatusChanged(object sender, bool status) {
            buttonStartStop.Invoke(new Action(() =>
                        buttonStartStop.Text = status ? "Stop" : "Start")
            );
            textBoxBotStatus.Invoke(new Action(() =>
                        textBoxBotStatus.Text = status ? "Bot Running" : "Bot not Running")
            );
        }

        private void textBoxLog_TextChanged(object s, EventArgs e) {
            var sender = s as RichTextBox;
            if (sender == null) return;
            sender.SelectionStart = sender.Text.Length;
            sender.ScrollToCaret();
        }

        private void buttonClearLog_Click(object sender, EventArgs e) {
            Logging.Clear();
        }

        private void buttonStartStop_Click(object sender, EventArgs e) {
            if (Core.IsRunning) Core.Stop();
            else {
                if (!Core.IsInGame) return;
                Core.Start();
            }
        }

        private void FormMainBot_Shown(object sender, EventArgs e) {
            Logging.Log("-- Loading Core --");
            if (!Core.Init()) {
                Logging.Log("[Error] Error loading Core.\nRestart Bot");
                return;
            }
            Logging.Log($"-- Core Loaded --");
        }

        private void button1_Click(object sender, EventArgs e) {
            if (!Core.IsInGame) return;
            Logging.Clear();
            //PacketManager.ClearTarget();
            var target = Core.Me.CurrentTarget ?? Core.Me;
            Logging.Log($"Casting Spell: {1381:X}\nTarget: {target}");
            PacketManager.UseSkillOnTargetId(1381, target.ObjectId);
        }
    }
}