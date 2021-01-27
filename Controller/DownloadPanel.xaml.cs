using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Inferno_Mod_Manager.Utils;

namespace Inferno_Mod_Manager.Controller
{
    /// <summary>
    /// Interaction logic for DownloadPanel.xaml
    /// </summary>
    public partial class DownloadPanel : UserControl
    {

        public Mod mdata = new Mod();

        public DownloadPanel()
        {
            InitializeComponent();
            DataContext = mdata;
        }
        public DownloadPanel(Mod mod)
        {
            InitializeComponent();
            DataContext = mod;
            mdata = mod;

            if (mod.PNGUrl != null && mod.PNGUrl != "" && mod.PNGUrl != "nothingYet")
            {
                var data = new WebClient().DownloadData(mod.PNGUrl);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(data);
                bitmap.DecodePixelHeight = 35;
                bitmap.DecodePixelWidth = 35;
                bitmap.EndInit();

                Image.Source = bitmap;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var m = mdata;

            var web = new WebClient();
            web.Headers.Add("user-agent", "Inferno Omnia");
            web.DownloadProgressChanged += (o, args) =>
            {
                MainWindow.Instance.progressBar.Value = args.ProgressPercentage;
            };
            web.DownloadFileCompleted += (o, args) =>
            {
                m.Type = m.Type.Contains("dll") ? "Melon Mod" : (m.Type.Contains("inferno") ? "Inferno Mod" : "BTD 6 Mod");
                ModManifest.Instance += m;
                MainWindow.Instance.RefreshDownloadsList();
                MainWindow.Instance.RefreshModList();
                MessageBox.Show($"Download for {m.Name} Completed!");
                MainWindow.Instance.progressBar.Value = 0;
            };

            m.CanonicalLocation = Storage.InstallDir + @"\Mods\" + m.Name + m.Type;
            web.DownloadFileAsync(new(m.DownloadUrl), m.CanonicalLocation);
        }

    }
}
