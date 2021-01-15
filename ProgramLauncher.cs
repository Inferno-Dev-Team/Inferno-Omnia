using Inferno_Mod_Manager.InfernoMods;
using Inferno_Mod_Manager.MelonMods;
using Inferno_Mod_Manager.Utils;
using Steamworks;
using System;
using System.IO;
using System.Windows;

namespace Inferno_Mod_Manager
{
    class ProgramLauncher
    {
        [STAThread]
        public static void Main(string[] args) {
            SteamClient.Init(960090);
            Storage.InstallDir = SteamApps.AppInstallDir();
            FileAssociations.EnsureAssociationsSet();
            _ = Directory.CreateDirectory(Storage.InstallDir + @"\Mods\Inferno");
            _ = Directory.CreateDirectory(Storage.InstallDir + @"\Mods\Inferno\Disabled");
            _ = Directory.CreateDirectory(Storage.InstallDir + @"\Mods");
            _ = Directory.CreateDirectory(Storage.InstallDir + @"\Mods\Disabled");
            _ = Directory.CreateDirectory(Environment.ExpandEnvironmentVariables("%AppData%\\InfernoModManager\\"));
            if (args.Length != 0)
                foreach (var file in args)
                    if (!file.Contains(@"\Mods\Inferno")) {
                        if (File.Exists(Storage.InstallDir + @"\Mods\Inferno\" + Path.GetFileName(file)))
                            File.Delete(Storage.InstallDir + @"\Mods\Inferno\" + Path.GetFileName(file));
                        File.Move(file, Storage.InstallDir + @"\Mods\Inferno\" + Path.GetFileName(file));
                    }

            if (Directory.GetFiles(Storage.InstallDir + @"\Mods\Inferno").Combine(Directory.GetFiles(Storage.InstallDir + @"\Mods\Inferno\Disabled")).Length > 0) {
                bool flag = false;
                foreach (var file in Directory.GetFiles(Storage.InstallDir + @"\Mods", "*.dll").Combine(Directory.GetFiles(Storage.InstallDir + @"\Mods\Inferno\Disabled"))) {
                    MelonHandler.GetMelonAttrib(file, out var att);
                    if (att != null)
                    flag |= att.Name.Equals("Inferno API Injector");
                }
                if (!flag)
                    File.Create(Storage.InstallDir + @"\Mods\Inferno API Injector.dll").Write(Properties.Resources.Inferno_API_Injector, 0, Properties.Resources.Inferno_API_Injector.Length);
            }
            var app = new App {ShutdownMode = ShutdownMode.OnMainWindowClose};
            app.InitializeComponent();
            app.Run();
        }
    }
}
