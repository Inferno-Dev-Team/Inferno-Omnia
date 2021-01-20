using Inferno_Mod_Manager.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Inferno_Mod_Manager.Utils
{
    internal class Storage
    {
        public static string InstallDir { get; set; }
        public static List<Mod> ModsList { get; set; }

        public static readonly double[] versions = ("7.1 7.2 8.1 9.0 9.1 10.0 10.1 10.2 11.0 11.1 11.2 12.0 12.1 12.2 12.3 13.0 13.1 " +
                                                    "14.0 14.1 14.2 15.0 15.1 15.2 16.0 16.1 16.2 17.0 17.1 18.0 18.1 19.0 19.1 19.2").Split(' ').ToDoubleArray();
        private static readonly string[] manifests = ("2054082021331818529 4596146723057622261 8467375375836982228 5602649970690083401 " +
                                                     "5035059096984096937 1441323002134250115 3151687751534146643 5741069292439127267 " +
                                                     "599319211439289280 3353959099250784350 4719427651645843200 6125580176822245534 " +
                                                     "2747258345428321016 8675922616859074834 2611418031133046417 6527766747861229054 " +
                                                     "2429406559233255474 7084106100824098541 5626402713560505517 2278385958666844203 " +
                                                     "4219620137794679233 3233661087155281553 3561468024947569898 8478831779476849055 " +
                                                     "2862522794566849764 4195411098836058324 59726612806897135 4009796719739046665 " +
                                                     "6026760569640491700 522753008708181218 6676853478479683553 6204358855064235447 2791323960385232130").Split(' ');
        private static readonly string[] offsets = ("2033216 2033216 3135328 3188496 3191280 2893408 2893952 2893952 3122080 3128224 " +
                                                    "3131952 3755504 3757296 3758320 3452528 3577424 3515664 [3551866,3552346] " +
                                                    "[3536586,3537066] [3537271,3537751] [3348583,3349063] [3348711,3349191] " +
                                                    "[3370303,3370783] [3337783,3338265] [3337799,3338281] [3337079,3337561] " +
                                                    "[3277399,3277881] [3394055,3394537] [4426535,4427017] [4428407,4428889] " +
                                                    "[4430074,4430489] [4435322,4435737] [4434474,4434889]").Split(' ');
        public static readonly Dictionary<double, string> manifestDict = versions.Zip(manifests, (ver, man) => new { ver, man }).ToDictionary(item => item.ver, item => item.man);
        public static readonly Dictionary<double, string> offsetDict = versions.Zip(offsets, (ver, off) => new { ver, off }).ToDictionary(item => item.ver, item => item.off);
        public static readonly string repo = Environment.ExpandEnvironmentVariables("%AppData%\\InfernoOmnia\\repo.json");
        public static readonly string mod = Environment.ExpandEnvironmentVariables("%AppData%\\InfernoOmnia\\mod.json");
        public static WebClient client = new();

        static Storage() {
            client.Headers.Add("user-agent", "Inferno Omnia");
        }
    }
}
