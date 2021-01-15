using System;
using System.Windows;
using DiscordRPC;
using DiscordRPC.Logging;
using Steamworks;

namespace Inferno_Mod_Manager {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public static DiscordRpcClient client;
        protected override void OnStartup(StartupEventArgs se)
        {
            client = new DiscordRpcClient("790509732561551402");
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Timestamps = Timestamps.Now,
                Details = "Playing Inferno Omnia",
                State = "Idle",
                Assets = new () {LargeImageKey = "main", LargeImageText = "Inferno - Omnia"}
            });
            var splashScreen = new SplashScreen("Resources/Inferno Splash Screen.png");
            splashScreen.Show(true);
            base.OnStartup(se);
        }

        protected override void OnExit(ExitEventArgs e) {
            client.Deinitialize();
            client.Dispose();
            base.OnExit(e);
        }

        public static RichPresence defaultRP = new ()
        {
            Timestamps = Timestamps.Now,
            Details = "Playing Inferno Omnia",
            State = "Idle",
            Assets = new () {LargeImageKey = "main", LargeImageText = "Inferno - Omnia"}
        };
        public static RichPresence btd6RP = new ()
        {
            Timestamps = Timestamps.Now,
            Details = "Playing Inferno Omnia",
            State = "In-Game",
            Assets = new () { LargeImageKey = "main", LargeImageText = "Inferno - Omnia", SmallImageKey = "btd6", SmallImageText = "Bloons TD 6" }
        };
        public static RichPresence battdRP = new ()
        {
            Timestamps = Timestamps.Now,
            Details = "Playing Inferno Omnia",
            State = "In-Game",
            Assets = new () { LargeImageKey = "main", LargeImageText = "Inferno - Omnia", SmallImageKey = "battd", SmallImageText = "Bloons Adventure Time TD" }
        };
        public static RichPresence btd6ModdedRP = new ()
        {
            Timestamps = Timestamps.Now,
            Details = "Playing Inferno Omnia",
            State = "In-Game",
            Assets = new () { LargeImageKey = "main", LargeImageText = "Inferno - Omnia", SmallImageKey = "btd6", SmallImageText = "Bloons TD 6 (Modded)" }
        };
        public static RichPresence battdModdedRP = new ()
        {
            Timestamps = Timestamps.Now,
            Details = "Playing Inferno Omnia",
            State = "In-Game",
            Assets = new () { LargeImageKey = "main", LargeImageText = "Inferno - Omnia", SmallImageKey = "battd", SmallImageText = "Bloons Adventure Time TD (Modded)" }
        };
    }
}
