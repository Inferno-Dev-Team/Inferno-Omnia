using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ModernWpf.Controls;

namespace Inferno_Mod_Manager.Controller
{
    /// <summary>
    /// Interaction logic for ModPanel.xaml
    /// </summary>
    public partial class ModPanel : UserControl
    {
        public Mod mdata = new Mod();

        public ModPanel()
        {
            InitializeComponent();
            DataContext = mdata;
        }
        public ModPanel(Mod m)
        {
            InitializeComponent();
            DataContext = m;
            mdata = m;
            if (m.Description != null && m.Description != "")
                textBlock.ToolTip = new ToolTip() { Content = m.Description + "\n\tBy: " + m.Author + "\n\tv" + m.Version };

            if (m.PNGUrl != null && m.PNGUrl != "" && m.PNGUrl != "nothingYet")
            {
                var data = new WebClient().DownloadData(m.PNGUrl);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(data);
                bitmap.DecodePixelHeight = 35;
                bitmap.DecodePixelWidth = 35;
                bitmap.EndInit();

                Image.Source = bitmap;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var button = sender as Button;
            var m = ((button.Parent as Grid).Parent as ModPanel).mdata;

            ModManifest.Instance -= ModManifest.Instance ^ m.Name;
            File.Delete(m.CanonicalLocation);
            MainWindow.Instance.RefreshDownloadsList();
            MainWindow.Instance.RefreshModList();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var checkBox = sender as ToggleSwitch;
            var m = ((checkBox.Parent as Grid).Parent as ModPanel).mdata;
            if (m.Enabled)
            {
                File.Move(m.CanonicalLocation, m.CanonicalLocation.Replace(@"\Disabled", ""));
                m.CanonicalLocation = m.CanonicalLocation.Replace(@"\Disabled", "");
            }
            else
            {
                if (!Directory.Exists(Path.GetDirectoryName(m.CanonicalLocation) + @"\Disabled"))
                    Directory.CreateDirectory(Path.GetDirectoryName(m.CanonicalLocation) + @"\Disabled");
                File.Move(m.CanonicalLocation, Path.GetDirectoryName(m.CanonicalLocation) + @"\Disabled\" + Path.GetFileName(m.CanonicalLocation));
                m.CanonicalLocation = Path.GetDirectoryName(m.CanonicalLocation) + @"\Disabled\" + Path.GetFileName(m.CanonicalLocation);
            }
        }
    }
}
