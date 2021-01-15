using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security;

namespace Inferno_Mod_Manager.InfernoMods {
    [SuppressUnmanagedCodeSecurity]
    public class FileAssociations
    {
        // needed so that Explorer windows get refreshed after the registry is updated
        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        public static void EnsureAssociationsSet() {
            if (SetAssociation())
                SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool SetAssociation() => SetKeyDefaultValue($@"Software\Classes\.inferno", "Inferno_Mod_Manager_Mod") |
                SetKeyDefaultValue($@"Software\Classes\Inferno_Mod_Manager_Mod", "Inferno Mod File") |
                SetKeyDefaultValue($@"Software\Classes\Inferno_Mod_Manager_Mod\shell\open\command", $"\"{Process.GetCurrentProcess().MainModule.FileName}\" \"%1\"");

        private static bool SetKeyDefaultValue(string keyPath, string value) {
            using (var key = Registry.CurrentUser.CreateSubKey(keyPath))
                if (key.GetValue(null) as string != value) {
                    key.SetValue(null, value);
                    return true;
                }

            return false;
        }
    }
}
