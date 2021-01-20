using System.Collections.Generic;
using System.Net;

namespace Inferno_Mod_Manager.Controller {
    public class WebDownloader {
        public static List<string> Repos { get; set; }

        public static List<string> GetAllData() {
            var compList = new List<string>();
            var web = new WebClient();
            web.Headers.Add("user-agent", "Inferno Omnia");
            web.Headers.Add("user", "IO");
            for (var i = 0; i < Repos.Count; i++) {
                var data = web.DownloadString(Repos[i]);
                data = data.Replace("\r", "");
                compList.Add(data);
            }

            return compList;
        }

        public static void IfBlankSet() {
            if (Repos == null || Repos.Count == 0 || Repos.Contains("https://raw.githubusercontent.com/Inferno-Dev-Team/Inferno-Mod-Manager/main/git.yo"))
                Repos = new(){ "https://raw.githubusercontent.com/Inferno-Dev-Team/Inferno-Omnia/main/api.json" };
        }
    }
}