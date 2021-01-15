using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using DiscordRPC;
using Inferno_Mod_Manager.Controller;
using Microsoft.Win32;

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

        // modified from https://stackoverflow.com/a/60354788/12427280
        public static bool IsVC2019x64Installed() {
            var dependenciesPath = @"SOFTWARE\Classes\Installer\Dependencies";

            using (var dependencies = Registry.LocalMachine.OpenSubKey(dependenciesPath)) {
                if (dependencies == null) return false;

                foreach (var subKeyName in dependencies.GetSubKeyNames().Where(n => !n.ToLower().Contains("dotnet") && !n.ToLower().Contains("microsoft"))) {
                    if (subKeyName.ContainsAll("VC", "redist"))
                        return true;
                }
            }

            return false;
        }
    }
}