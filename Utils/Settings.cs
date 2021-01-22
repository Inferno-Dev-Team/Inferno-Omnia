using System.IO;
using Newtonsoft.Json;

namespace Inferno_Mod_Manager.Utils {
    public class Settings {
        private bool shownVCScreen = false;
        public bool ShownVCScreen {
            get {
                if (shownVCScreen == null)
                    shownVCScreen = false;
                return shownVCScreen;
            }
            set {
                shownVCScreen = value;
                Storage.Settings = this;
                rewriteFile();
            }
        }

        public void rewriteFile() => File.WriteAllText(Storage.usr, JsonConvert.SerializeObject(this, Formatting.Indented));
    }
}