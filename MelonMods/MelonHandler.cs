
using MelonLoader;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using Inferno_Mod_Manager.Utils;
using Newtonsoft.Json;
using System.Threading;
using System.Diagnostics;

namespace Inferno_Mod_Manager.MelonMods {
    class MelonHandler {
        public static void GetMelonAttrib(string filePath, out MelonInfoAttribute melonItemAttribute) {
            melonItemAttribute = null;
            try {
                var asm = Assembly.Load(File.ReadAllBytes(filePath));
                var attribs = Attribute.GetCustomAttributes(asm);
                foreach (var attrib in attribs)
                    if (attrib is MelonInfoAttribute)
                        melonItemAttribute = attrib as MelonInfoAttribute;
            } catch (Exception e) {
                Console.Write(e);
                return;
            }
        }

        public static void EnsureMelonInstalled() {
            if (!File.Exists(Storage.Version)) {
                new Thread(() => {
                    dynamic dyn = JsonConvert.DeserializeObject(Storage.client.DownloadString("https://api.github.com/repos/HerpDerpinstine/MelonLoader/releases"));
                    var downloadLink = dyn[0].assets[3].browser_download_url.ToString();
                    try {
                        Storage.client.DownloadFile(downloadLink, Storage.Temp);
                        System.IO.Compression.ZipFile.ExtractToDirectory(Storage.Temp, Storage.Dir.Install.Path);
                        File.Delete(Storage.Temp);
                        MessageBox.Show("Finished Downloading MelonLoader!");
                    } catch (Exception e) {
                        MessageBox.Show(e.Message + "\nPlease manually install melonloader from this link: " + downloadLink, "Automatic Install Failed");
                    }
                }) { IsBackground = true }.Start();
                MessageBox.Show("Can't detect MelonLoader! Installing now...");
            }
        }
    }
}
