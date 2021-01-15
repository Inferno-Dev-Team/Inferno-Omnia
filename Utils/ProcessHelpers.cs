using System.Diagnostics;
using DiscordRPC;
using Inferno_Mod_Manager.Controller;

namespace Inferno_Mod_Manager.Utils {
    public class ProcessHelpers {
        public static void RunWithRPC(ProcessStartInfo psi, RichPresence newRPC) {
            var process = new Process();
            process.StartInfo = psi;
            process.EnableRaisingEvents = true;
            process.Exited += (o, args) => App.client.SetPresence(App.defaultRP);
            App.client.SetPresence(newRPC);
            process.Start();
        }
    }
}