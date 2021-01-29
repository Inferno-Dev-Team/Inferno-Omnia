using System;
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
        public Mod mdata = new();

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

            if (!string.IsNullOrWhiteSpace(m.PNGUrl) && m.PNGUrl != "nothingYet")
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

            if (string.IsNullOrWhiteSpace(m.Description)) {
                VerTextBlck.Text = string.Empty;
                MExpander.Content = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var m = mdata;

            ModManifest.Instance -= ModManifest.Instance ^ m.Name;
            File.Delete(m.CanonicalLocation);
            MainWindow.Instance.RefreshDownloadsList();
            MainWindow.Instance.RefreshModList();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            var m = mdata;
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
