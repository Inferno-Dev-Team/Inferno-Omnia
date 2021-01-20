using Inferno_Mod_Manager.MelonMods;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inferno_Mod_Manager.Controller
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Mod : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name = "???", _type = ".dll";
        private string author, version = "1.0", description, tags, downloadUrl, pngUrl;
        private bool _enabled = true;
        private string canonicalLocation = "";
        public string CanonicalLocation
        {
            get => canonicalLocation;
            set { canonicalLocation = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string Type
        {
            get => _type;
            set { _type = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string Version
        {
            get => version;
            set { version = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string Author
        {
            get => author;
            set { author = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string Description
        {
            get => description;
            set { description = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string Tags
        {
            get => tags;
            set { tags = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string DownloadUrl
        {
            get => downloadUrl;
            set { downloadUrl = value; NotifyPropertyChanged(); }
        }
        [JsonProperty]
        public string PNGUrl
        {
            get => pngUrl;
            set { pngUrl = value; NotifyPropertyChanged(); }
        }

        public bool Enabled
        {
            get => _enabled;
            set { _enabled = value; NotifyPropertyChanged(); }
        }
        public string EnabledString
        {
            get => _enabled ? "Enabled" : "Disabled";
        }
    }
}
