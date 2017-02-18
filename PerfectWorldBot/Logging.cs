using System;
using System.Windows.Forms;

namespace PerfectWorldBot {
    public static class Logging {
        private static Control LogControl;
        private static bool useCtrl;

        public static void Log(string msg) {
            if (useCtrl) {
                msg = msg + "\n";
                var ctrl = LogControl as RichTextBox;
                if (ctrl != null)
                    Invoke(() => ctrl.AppendText(msg));
                else
                    throw new ApplicationException("Logging Error: Log Control is null");
            } else {
                Console.WriteLine(msg);
            }
        }

        public static void Clear() {
            if (useCtrl) {
                var ctrl = LogControl as RichTextBox;
                if (ctrl != null)
                    Invoke(() => ctrl.Clear());
                else
                    throw new ApplicationException("Logging Error: Log Control is null");
            } else {
                Console.Clear();
            }
        }

        public static void SetLogControl(Control ctrl) {
            if (ctrl != null) {
                LogControl = ctrl;
                useCtrl = true;
            } else {
                LogControl = null;
                useCtrl = false;
            }
        }

        public static void SetLogControl() {
            LogControl = null;
            useCtrl = false;
        }

        public static void LogConsole(string msg) {
            Console.WriteLine(msg);
        }

        private static void Invoke(Action method) {
            try {
                if (LogControl.InvokeRequired)
                    LogControl.Invoke(method);
                else
                    method();
            } catch (Exception ex) {
                LogConsole($"[Error] {ex.Message}");
            }
        }
    }
}