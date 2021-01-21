using System;
using System.IO;
using Steamworks;
using System.Windows;
using Inferno_Mod_Manager.Controller;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using Inferno_Mod_Manager.Utils;
using Microsoft.Win32;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Security;
using Inferno_Mod_Manager.MelonMods;

namespace Inferno_Mod_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        public MainWindow()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Instance = this;

            WebDownloader.IfBlankSet();
            WebDownloader.GetAllData();
            SetupRepo();

            RefreshModList();
            RefreshDownloadsList();
            RefreshAchievements();
        }

        public void RefreshModList()
        {
            stackPanel.Children.Clear();
            var files = Directory.GetFiles(Storage.InstallDir + @"\Mods", "*.dll").Combine(Directory.GetFiles(Storage.InstallDir + @"\Mods\Inferno", "*.inferno"));
            for (var i = 0; i < files.Length; i++)
            {
                var fvi = FileVersionInfo.GetVersionInfo(files[i]);
                if (fvi == null || fvi.OriginalFilename == null) {
                    var m = new Mod
                    {
                        Name = Path.GetFileNameWithoutExtension(files[i]),
                        Type = Path.GetExtension(files[i]).Contains("dll") ? "Melon Mod" : Path.GetExtension(files[i]).Contains("inferno") ? "Inferno Mod" : "BTD 6 Mod",
                        CanonicalLocation = files[i],
                        Enabled = true
                    };
                    stackPanel.Children.Add(new ModPanel(m));
                    continue;
                }
                if ((ModManifest.Instance ^ Path.GetFileNameWithoutExtension(files[i])) != ModManifest.TemplateMod)
                {
                    var m = ModManifest.Instance ^ Path.GetFileNameWithoutExtension(files[i]);
                    m.CanonicalLocation = files[i];
                    m.Enabled = true;
                    stackPanel.Children.Add(new ModPanel(m));
                }
                else if ((ModManifest.Instance ^ fvi.OriginalFilename.Split('.')[0]) != ModManifest.TemplateMod)
                {
                    var m = ModManifest.Instance ^ fvi.OriginalFilename.Split('.')[0];
                    m.CanonicalLocation = files[i];
                    m.Enabled = true;
                    stackPanel.Children.Add(new ModPanel(m));
                }
                else
                {
                    var m = new Mod
                    {
                        Name = Path.GetFileNameWithoutExtension(files[i]),
                        Type = Path.GetExtension(files[i]).Contains("dll") ? "Melon Mod" : (Path.GetExtension(files[i]).Contains("inferno") ? "Inferno Mod" : "BTD 6 Mod"),
                        CanonicalLocation = files[i],
                        Enabled = true
                    };
                    stackPanel.Children.Add(new ModPanel(m));
                }
            }

            if (Directory.Exists(Storage.InstallDir + @"\Mods\Disabled"))
            {
                var disabledFiles = Directory.GetFiles(Storage.InstallDir + @"\Mods\Disabled", "*.dll").Combine(Directory.GetFiles(Storage.InstallDir + @"\Mods\Inferno\Disabled", "*.inferno"));
                for (var i = 0; i < disabledFiles.Length; i++)
                {
                    var fvi = FileVersionInfo.GetVersionInfo(disabledFiles[i]);
                    if (fvi == null || fvi.OriginalFilename == null) {
                        var m = new Mod
                        {
                            Name = Path.GetFileNameWithoutExtension(disabledFiles[i]),
                            Type = Path.GetExtension(disabledFiles[i]).Contains("dll") ? "Melon Mod" : (Path.GetExtension(disabledFiles[i]).Contains("inferno") ? "Inferno Mod" : "BTD 6 Mod"),
                            CanonicalLocation = disabledFiles[i],
                            Enabled = false
                        };
                        stackPanel.Children.Add(new ModPanel(m));
                        continue;
                    }
                    if ((ModManifest.Instance ^ Path.GetFileNameWithoutExtension(disabledFiles[i])) != ModManifest.TemplateMod)
                    {
                        var m = ModManifest.Instance ^ Path.GetFileNameWithoutExtension(disabledFiles[i]);
                        m.Enabled = false;
                        m.CanonicalLocation = disabledFiles[i];
                        stackPanel.Children.Add(new ModPanel(m));
                    }
                    else if ((ModManifest.Instance ^ fvi.OriginalFilename.Split('.')[0]) != ModManifest.TemplateMod)
                    {
                        var m = ModManifest.Instance ^ fvi.OriginalFilename.Split('.')[0];
                        m.Enabled = false;
                        m.CanonicalLocation = disabledFiles[i];
                        stackPanel.Children.Add(new ModPanel(m));
                    }
                    else
                    {
                        var m = new Mod
                        {
                            Name = Path.GetFileNameWithoutExtension(disabledFiles[i]),
                            Type = Path.GetExtension(disabledFiles[i]).Contains("dll") ? "Melon Mod" : (Path.GetExtension(disabledFiles[i]).Contains("inferno") ? "Inferno Mod" : "BTD 6 Mod"),
                            CanonicalLocation = disabledFiles[i],
                            Enabled = false
                        };
                        stackPanel.Children.Add(new ModPanel(m));
                    }
                }
            }
        }

        public void RefreshDownloadsList()
        {
            stackPanelDownload.Children.Clear();
            try {
                foreach (var a in WebDownloader.GetAllData()) {
                    var aa = JsonConvert.DeserializeObject<List<Mod>>(a, new JsonSerializerSettings{DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
                    foreach (var aaa in aa) if (aaa != null) Storage.ModsList.Add(aaa);
                }
            } catch (Exception e) {
                //MessageBox.Show($"Error! {e.GetType().FullName}", "Clearing cached repos");
                WebDownloader.Repos = new();
                WebDownloader.IfBlankSet();
                foreach (var a in WebDownloader.GetAllData()) {
                    var aa = JsonConvert.DeserializeObject<List<Mod>>(a, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    foreach (var aaa in aa) if (aaa != null) Storage.ModsList.Add(aaa);
                }
            }
            for (var i = 0; i < Storage.ModsList.Count; i++)
            {
                var modData = Storage.ModsList[i];

                if ((ModManifest.Instance ^ modData.Name) == null || (ModManifest.Instance ^ modData.Name) == ModManifest.TemplateMod) // First part is prolly always gonna be true.
                    stackPanelDownload.Children.Add(new DownloadPanel(modData));
            }
        }

        public void RefreshAchievements() {
            achievementsTreeView.Items.Clear();
            foreach (var achievement in SteamUserStats.Achievements)
            {
                var ap = new AchievementPanel() { ToolTip = "???" };
                ap.__As_0x32459 = achievement;
                ap.achName = achievement.Name;
                if (achievement.GetIcon().HasValue)
                    try
                    {
                        var img = achievement.GetIcon().Value;
                        var rgba = new RGBASource(img.Data, checked((int)img.Width));
                        if (!achievement.State)
                            for (var i = 0; i < img.Data.Length; i += 4)
                            {
                                var gray = (byte)((img.Data[i] + img.Data[i + 1] + img.Data[i + 2]) / 3);
                                img.Data[i] = gray;
                                img.Data[i + 1] = gray;
                                img.Data[i + 2] = gray;
                            }

                        ap.achievementName.Text = new StringBuilder(achievement.Name).ToString();
                        ap.achievementName.Foreground = new SolidColorBrush(Colors.White);
                        ap.ToolTip = new ToolTip() { Content = achievement.Description };
                        ap.achievementImage.Source = rgba;
                        achievementsTreeView.Items.Add(ap);
                        continue;
                    }
                    catch (Exception) {achievementsTreeView.Items.Add(ap);}
                ap.achievementName.Text = achievement.Name;
                ap.achievementName.Foreground = new SolidColorBrush(Colors.White);
                ap.ToolTip = new ToolTip() { Content = achievement.Description };
                achievementsTreeView.Items.Add(ap);
            }
            achievementsTreeView.Items.SortDescriptions.Add(new("achName", ListSortDirection.Ascending));
        }

        private void SetupRepo()
        {
            if (!File.Exists(Storage.repo))
            {
                File.Create(Storage.repo).Close();
                File.WriteAllText(Storage.repo, JsonConvert.SerializeObject(WebDownloader.Repos));
            }
            else {WebDownloader.Repos = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(Storage.repo));}
        }

        private void Window_Closing(object sender, CancelEventArgs e) => SteamClient.Shutdown();

        private void Add_Repo_Button_Click(object sender, RoutedEventArgs e) {
            try {
                var inputDialog = new OneAnswerInputDialogWindow("Please enter a new Repo:");
                if (inputDialog.ShowDialog() == true) {
                    WebDownloader.Repos.Add(inputDialog.Answer);
                } else {
                    MessageBox.Show("Repo Addition Cancelled!");
                    RefreshModList();
                    return;
                }

                File.WriteAllText(Storage.repo, JsonConvert.SerializeObject(WebDownloader.Repos));
                RefreshDownloadsList();
            }
            catch (Exception a) {MessageBox.Show("Couldn't Add the repo!\n" + a.Message, "ERROR!");}
        }

        private void Add_Mod_Button_Click(object sender, RoutedEventArgs e)
        {
            string Name, Author, Description, Tags, PNGUrl, Version;
            try
            {
                var inputDialogName = new InputDialogSample("Please enter the Name of the mod:", "Please enter the Description of the mod:", "Please enter the Author of the mod:", "Please enter the Tags of the mod:", "Please enter the Version number of the mod:", "Please enter the URL for the image of the mod (If there is not one leave blank):");
                if (inputDialogName.ShowDialog() ?? true)
                {
                    Name = inputDialogName.Answer;
                    Description = inputDialogName.Answer2;
                    Author = inputDialogName.Answer3;
                    Tags = inputDialogName.Answer4;
                    Version = inputDialogName.Answer5;
                    PNGUrl = inputDialogName.Answer6;
                } else {
                    MessageBox.Show("Mod Addition Cancelled!");
                    return;
                }

                var file = BrowseForFile("Please select the mod to import!");

                if (Path.GetExtension(file).Contains("dll")) {
                    MelonHandler.GetMelonAttrib(file, out var att);
                    if (att != null)
                        if ((ModManifest.Instance ^ att.Name) != ModManifest.TemplateMod || (ModManifest.Instance ^ Path.GetFileName(file)) != ModManifest.TemplateMod)
                            throw new("Mod already exists!");
                }

                var dd = new Mod { Name = Name, Author = Author, Description = Description, Type = Path.GetExtension(file), DownloadUrl = "null.com", PNGUrl = PNGUrl, Tags = Tags, Version = Version };
                File.Copy(file, Storage.InstallDir + @"\Mods\" + Name + Path.GetExtension(file));
                ModManifest.Instance += dd;
                RefreshModList();
            }
            catch (Exception a) { MessageBox.Show("Couldn't Add the mod!\n" + a.Message, "ERROR!"); }
        }

        public static string BrowseForFile(string title)
        {
            var fileDiag = new OpenFileDialog
            {
                Title = title,
                DefaultExt = "dll",
                Filter = "Dynamic Link Library(*.dll)|*.dll|Inferno API Mods|*.inferno",
                Multiselect = false
            };

            if (fileDiag.ShowDialog() ?? true)
                return fileDiag.FileName;
            else
                return "";
        }

        private void LaunchGame_Click(object sender, RoutedEventArgs e) => ProcessHelpers.RunWithRPC(new(Storage.InstallDir + @"\BloonsTD6.exe"), App.btd6ModdedRP);

        private void LaunchGameNoMods_Click(object sender, RoutedEventArgs e) => ProcessHelpers.RunWithRPC(new(Storage.InstallDir + @"\BloonsTD6.exe", "--no-mods"), App.btd6RP);

        private void Window_Loaded(object sender, RoutedEventArgs e) => InfernoChecker.PI_as_0x000003(InfernoChecker.PI_as_0x000002(InfernoChecker.PI_as_0x000001()));

        private void DiscordButton_Click(object sender, RoutedEventArgs e) => Process.Start("https://discord.gg/D7v6h3KSQN");
    }
}
