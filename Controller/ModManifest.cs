using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Inferno_Mod_Manager.Utils;
using Newtonsoft.Json;

namespace Inferno_Mod_Manager.Controller {
    internal class ModManifest {
        public static ModManifest Instance = new();
        public static Mod TemplateMod = new();

        public static ModManifest operator +(ModManifest mm, Mod m) {
            if (!File.Exists(Storage.Mod))
                File.Create(Storage.Mod).Close();
            var mods = mm*mm.GetType();
            mods.Add(m);
            File.WriteAllText(Storage.Mod, JsonConvert.SerializeObject(mods, Formatting.Indented));
            return mm;
        }

        public static ModManifest operator -(ModManifest mm, Mod m) {
            if (!File.Exists(Storage.Mod) || m == null || m == TemplateMod)
                return mm;

            var mods = mm*mm.GetType();
            var usableMod = mods.First(k => k.Name.Equals(m.Name) && k.CanonicalLocation.Equals(m.CanonicalLocation));
            if (!mods.Remove(usableMod))
                MessageBox.Show("Couldn't Fully Remove Mod!");

            File.WriteAllText(Storage.Mod, JsonConvert.SerializeObject(mods, Formatting.Indented));
            return mm;
        }

        public static Mod operator ^(ModManifest mm, string s) {
            if (!File.Exists(Storage.Mod))
                return TemplateMod;

            var mods = mm*mm.GetType();
            for (var i = 0; i < mods.Count; i++)
                if (mods[i].Name.ToLower().Equals(s.ToLower()))
                    return mods[i];

            return TemplateMod;
        }

        public static List<Mod> operator *(ModManifest mm, Type type) {
            var mods = JsonConvert.DeserializeObject<List<Mod>>(File.ReadAllText(Storage.Mod));

            if (mods == null)
                mods = new();

            return mods;
        }
    }
}