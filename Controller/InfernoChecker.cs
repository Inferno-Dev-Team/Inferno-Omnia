using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Inferno_Mod_Manager.Controller
{
    class InfernoChecker
    {
        private const string InfernoChecker_ = "InfernoCheckers.dll";

        [DllImport(InfernoChecker_)]
        private static extern float as_0x000000();

        [DllImport(InfernoChecker_)]
        private static extern bool as_0x000001();

        [DllImport(InfernoChecker_)]
        private static extern bool as_0x000002(bool a);

        [DllImport(InfernoChecker_)]
        private static extern void as_0x000003(bool a);

        [DllImport(InfernoChecker_)]
        private static extern unsafe byte* as_0x000004();

        //-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~-~

        public static float PI_as_0x000000() => as_0x000000();
        public static bool PI_as_0x000001() => as_0x000001();
        public static bool PI_as_0x000002(bool a) => as_0x000002(a);
        public static void PI_as_0x000003(bool a) => as_0x000003(a);
        public static unsafe byte* PI_as_0x000004(bool a, object o, int e, ModPanel f) => as_0x000004();
    }
}