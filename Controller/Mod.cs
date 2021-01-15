using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Inferno_Mod_Manager.Controller
{
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
        public string Name
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged(); }
        }
        public string Type
        {
            get => _type;
            set { _type = value; NotifyPropertyChanged(); }
        }
        public string Version
        {
            get => version;
            set { version = value; NotifyPropertyChanged(); }
        }
        public string Author
        {
            get => author;
            set { author = value; NotifyPropertyChanged(); }
        }
        public string Description
        {
            get => description;
            set { description = value; NotifyPropertyChanged(); }
        }
        public string Tags
        {
            get => tags;
            set { tags = value; NotifyPropertyChanged(); }
        }
        public string DownloadUrl
        {
            get => downloadUrl;
            set { downloadUrl = value; NotifyPropertyChanged(); }
        }
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
