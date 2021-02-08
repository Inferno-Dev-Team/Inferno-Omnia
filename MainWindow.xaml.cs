using System;
using System.IO;
using Steamworks;
using System.Windows;
using Inferno_Mod_Manager.Controller;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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

        public Mod MakeNewMod(string path, bool enabled, string name = "") => new() {
            Name = name.Equals("") ? Path.GetFileNameWithoutExtension(path) : name,
            Type = Path.GetExtension(path),
            CanonicalLocation = path,
            Enabled = enabled
        };

        public void ChangeExistingMod(Mod mod, string path, bool enabled) {
            mod.CanonicalLocation = path;
            mod.Enabled = enabled;
        }

        public void RefreshModList() {
            ModList.Children.Clear();
            var modFiles = Storage.GetModFiles(Storage.ModDir.Mods, Storage.ModDir.Inferno);
            for (var i = 0; i < modFiles.Length; i++) {
                Mod mod;
                var fvi = FileVersionInfo.GetVersionInfo(modFiles[i]);
                if (fvi == null || fvi.OriginalFilename == null)
                    mod = MakeNewMod(modFiles[i], true);
                else {
                    mod = ModManifest.Instance ^ Path.GetFileNameWithoutExtension(modFiles[i]);
                    if (mod != ModManifest.TemplateMod)
                        ChangeExistingMod(mod, modFiles[i], true);
                    else {
                        mod = ModManifest.Instance ^ fvi.OriginalFilename.Split('.')[0];
                        if (mod != ModManifest.TemplateMod)
                            ChangeExistingMod(mod, modFiles[i], true);
                        else
                            mod = MakeNewMod(modFiles[i], true);
                    }
                }
            }

            var disableModFiles = Storage.GetModFiles(Storage.ModDir.DisabledMods, Storage.ModDir.DisabledInferno);
            for (var i = 0; i < disableModFiles.Length; i++) {
                Mod disabledMod;
                var fvi = FileVersionInfo.GetVersionInfo(disableModFiles[i]);
                if (fvi == null || fvi.OriginalFilename == null)
                    disabledMod = MakeNewMod(disableModFiles[i], false);
                else {
                    disabledMod = ModManifest.Instance ^ Path.GetFileNameWithoutExtension(disableModFiles[i]);
                    if (disabledMod != ModManifest.TemplateMod)
                        ChangeExistingMod(disabledMod, disableModFiles[i], false);
                    else {
                        disabledMod = ModManifest.Instance ^ fvi.OriginalFilename.Split('.')[0];
                        if (disabledMod != ModManifest.TemplateMod)
                            ChangeExistingMod(disabledMod, disableModFiles[i], false);
                        else
                            disabledMod = MakeNewMod(disableModFiles[i], false);
                    }
                }
            }
        }

        private void ParseDownloadsList() {
            WebDownloader.IfBlankSet();
            foreach (var a in WebDownloader.GetAllData()) {
                var aa = JsonConvert.DeserializeObject<List<Mod>>(a, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore, NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                foreach (var aaa in aa)
                    if (aaa != null)
                        Storage.ModsList.Add(aaa);
            }
        }

        public void RefreshDownloadsList()
        {
            stackPanelDownload.Children.Clear();
            try {
                ParseDownloadsList();
            } catch (Exception e) {
                MessageBox.Show($"Error! {e.GetType().FullName}", "Clearing cached repos");
                WebDownloader.Repos = new();
                ParseDownloadsList();
            }
            for (var i = 0; i < Storage.ModsList.Count; i++)
            {
                var modData = Storage.ModsList[i];

                if ((ModManifest.Instance ^ modData.Name) == ModManifest.TemplateMod)
                    DownloadList.Children.Add(new DownloadPanel(modData));
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

        public static void SetupSettings()
        {
            if (!File.Exists(Storage.usr))
            {
                File.Create(Storage.usr).Close();
                File.WriteAllText(Storage.usr, JsonConvert.SerializeObject(Storage.Settings));
            }
            else {Storage.Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Storage.usr));}
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
