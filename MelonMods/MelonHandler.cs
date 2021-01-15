
using MelonLoader;
using System;
using System.IO;
using System.Reflection;
using Inferno_Mod_Manager.Utils;

namespace Inferno_Mod_Manager.MelonMods
{
    class MelonHandler
    {
        public static void GetMelonAttrib(string filePath, out MelonInfoAttribute melonItemAttribute)
        {
            melonItemAttribute = null;
            try {
                var asm = Assembly.Load(File.ReadAllBytes(filePath)); /*Storage.domain.Load(File.ReadAllBytes(filePath))*/;
                var attribs = Attribute.GetCustomAttributes(asm);
                foreach (var attrib in attribs)
                    if (attrib is MelonInfoAttribute)
                        melonItemAttribute = attrib as MelonInfoAttribute;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return;
            }
        }
    }
}
