using System.Collections.Generic;
using System.Net;

namespace Inferno_Mod_Manager.Controller {
    public class WebDownloader {
        public static List<string> Repos { get; set; }

        public static List<string> GetAllData() {
            var compList = new List<string>();
            var web = new WebClient();
            web.Headers.Add("user-agent", "Inferno Mod Manager");
            web.Headers.Add("user", "IMM");
            for (var i = 0; i < Repos.Count; i++) {
                var data = web.DownloadString(Repos[i]);
                data.Replace("\r", "");
                for (var j = 0; j < data.Split('\n').Length; j++)
                    compList.Add(data.Split('\n')[j]);
            }

            return compList;
        }

        public static void IfBlankSet() {
            if (Repos == null || Repos.Count == 0)
                Repos = new(){"https://raw.githubusercontent.com/Inferno-Dev-Team/Inferno-Mod-Manager/main/git.yo"};
        }
    }
}