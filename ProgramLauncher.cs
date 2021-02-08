using Inferno_Mod_Manager.InfernoMods;
using Inferno_Mod_Manager.MelonMods;
using Inferno_Mod_Manager.Utils;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Inferno_Mod_Manager.Controller;
using Inferno_Mod_Manager.Properties;

namespace Inferno_Mod_Manager {
    class ProgramLauncher {
        public static bool IsDebugRelease
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        [STAThread]
        public static void Main(string[] args) {
            if (IsDebugRelease)
                MessageBox.Show("Code messed up somewhere, report this to the devs, PROGRAM RUN IN DEBUG");

            MainWindow.SetupSettings();
            if (!ProcessHelpers.IsVC2019x64Installed() && !Storage.Settings.ShownVCScreen) {
                MessageBox.Show("You do not have Visual C installed! Continue to install it.\nTHIS WILL NOT BE SHOWN AGAIN!\nDO NOT REPORT THE APP NOT RUNNING IF YOU DIDNT INSTALL THIS");
                Process.Start("https://aka.ms/vs/16/release/VC_redist.x64.exe");
                Storage.Settings.ShownVCScreen = true;
                throw new("Need Visual C Installed!");
            }

            var mtr = new List<Mod>();
            foreach (var a in ModManifest.Instance * typeof(Mod))
                if (!File.Exists((ModManifest.Instance ^ a.Name).CanonicalLocation))
                    mtr.Add(a);

            foreach (var remove in mtr) ModManifest.Instance -= remove;

            if (args.Length != 0)
                foreach (var file in args)
                    if (!file.Contains(Storage.ModDir.Inferno.Path)) {
                        string newFile = Storage.ModDir.Inferno.Path + Path.GetFileName(file);
                        if (File.Exists(newFile))
                            File.Delete(newFile);
                        File.Move(file, newFile);
                    }

            if (Storage.GetModFiles(Storage.ModDir.Inferno, Storage.ModDir.DisabledInferno).Length > 0) {
                var flag = false;
                foreach (var file in Storage.GetModFiles(Storage.ModDir.Mods, Storage.ModDir.DisabledMods)) {
                    MelonHandler.GetMelonAttrib(file, out var att);
                    if (att != null)
                        flag |= att.Name.Equals("Inferno API Injector");
                }
                if (!flag)
                    File.Create(Storage.ModDir.Mods + @"\Inferno API Injector.dll").Write(Resources.Inferno_API_Injector, 0, Resources.Inferno_API_Injector.Length);
            }
            var app = new App { ShutdownMode = ShutdownMode.OnMainWindowClose };
            app.InitializeComponent();
            app.Run();
        }
    }
}
