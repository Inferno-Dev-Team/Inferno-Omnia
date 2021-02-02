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
        [STAThread]
        public static void Main(string[] args) {
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
                    if (!file.Contains(ModDir.Inferno.Dir)) {
                        string newFile = ModDir.Inferno.Dir + Path.GetFileName(file);
                        if (File.Exists(newFile))
                            File.Delete(newFile);
                        File.Move(file, newFile);
                    }

            if (Storage.GetModFiles(ModDir.Inferno, ModDir.DisabledInferno).Length > 0) {
                var flag = false;
                foreach (var file in Storage.GetModFiles(ModDir.Mods, ModDir.DisabledMods)) {
                    MelonHandler.GetMelonAttrib(file, out var att);
                    if (att != null)
                        flag |= att.Name.Equals("Inferno API Injector");
                }
                if (!flag)
                    File.Create(ModDir.Mods + @"\Inferno API Injector.dll").Write(Resources.Inferno_API_Injector, 0, Resources.Inferno_API_Injector.Length);
            }
            var app = new App { ShutdownMode = ShutdownMode.OnMainWindowClose };
            app.InitializeComponent();
            app.Run();
        }
    }
}
