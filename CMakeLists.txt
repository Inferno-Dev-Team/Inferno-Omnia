cmake_minimum_required(VERSION 3.17)
project(InfernoOmnia VERSION 1.2.0 LANGUAGES CSharp)

include(CSharpUtilities)

add_executable(InfernoOmnia
    App.config
    App.xaml
    App.xaml.cs
    MainWindow.xaml
    MainWindow.xaml.cs
    ProgramLauncher.cs
    Controller/AchievementPanel.xaml
    Controller/AchievementPanel.xaml.cs
    Controller/DownloadPanel.xaml.cs
    Controller/DownloadPanel.xaml
    Controller/Game.cs
    Controller/InfernoChecker.cs
    Controller/InputDialogWindow.xaml
    Controller/InputDialogWindow.xaml.cs
    Controller/Mod.cs
    Controller/ModManifest.cs
    Controller/ModPanel.xaml.cs
    Controller/ModPanel.xaml
    Controller/OneAnswerInputDialogWindow.xaml
    Controller/OneAnswerInputDialogWindow.xaml.cs
    Controller/ProfilePanel.xaml
    Controller/ProfilePanel.xaml.cs
    Controller/Rand.cs
    Controller/SupportedGames.cs
    Controller/WebDownloader.cs
    InfernoMods/FileAssociation.cs
    MelonMods/MelonHandler.cs
    Utils/Extensions.cs
    Utils/ProcessHelpers.cs
    Utils/RGBASource.cs
    Utils/Settings.cs
    Utils/Storage.cs

    Properties/AssemblyInfo.cs
    Properties/Resources.Designer.cs
    Properties/Resources.resx
    Properties/Settings.Designer.cs
    Properties/Settings.cs
    Properties/Settings.settings)

csharp_set_designer_cs_properties(
	Properties/AssemblyInfo.cs
	Properties/Resources.Designer.cs
	Properties/Resources.resx
	Properties/Settings.Designer.cs
    	Properties/Settings.cs
	Properties/Settings.settings)

csharp_set_xaml_cs_properties(
	App.xaml
	App.xaml.cs
	MainWindow.xaml
	MainWindow.xaml.cs
    	Controller/AchievementPanel.xaml
    	Controller/AchievementPanel.xaml.cs
    	Controller/DownloadPanel.xaml.cs
    	Controller/DownloadPanel.xaml
    	Controller/InputDialogWindow.xaml
    	Controller/InputDialogWindow.xaml.cs
    	Controller/ModPanel.xaml.cs
    	Controller/ModPanel.xaml
    	Controller/OneAnswerInputDialogWindow.xaml
    	Controller/OneAnswerInputDialogWindow.xaml.cs
    	Controller/ProfilePanel.xaml
    	Controller/ProfilePanel.xaml.cs)

set_property(TARGET InfernoOmnia PROPERTY VS_DOTNET_TARGET_FRAMEWORK_VERSION "v4.8")
set_property(TARGET InfernoOmnia PROPERTY WIN32_EXECUTABLE TRUE)

set_property(TARGET InfernoOmnia PROPERTY VS_DOTNET_REFERENCES
    "Microsoft.CSharp"
    "PresentationCore"
    "PresentationFramework"
    "System"
    "System.Core"
    "System.Data"
    "System.Data.DataSetExtensions"
    "System.Net.Http"
    "System.Xaml"
    "System.Xml"
    "System.Xml.Linq"
    "WindowsBase"
    "DiscordRPC"
    "DotNetZip"
    "Facepunch.Steamworks.Win64"
    "FluentWPF"
    "MelonLoader.ModHandler"
    "ModernWpf.Controls"
    "ModernWpf"
    "Newtonsoft.Json")