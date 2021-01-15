using System;

namespace Inferno_Mod_Manager.Controller
{
    class Rand : Random
    {
        public static bool operator /(Rand r, Random e) => true;
        public static bool operator ^(Rand r, Action a) => false;
    }
}
