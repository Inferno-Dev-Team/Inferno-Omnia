using Newtonsoft.Json;

namespace InfernoOmnia.Mods
{
    #nullable enable
    public struct Mod
    {
        public string Name;
        public string Author;
        public string Type;
        public decimal Version;
        public string Description;
        public string Tags;
        public string DownloadUrl;
        public string PNGUrl;
        public bool? Enabled;
        public string? CanonicalLocation;

        public override string ToString() => JsonConvert.SerializeObject(this, settings);
        public void FromString(string s) => this = JsonConvert.DeserializeObject<Mod>(s, settings);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Doesnt need to be readonly as VS will whine")]
        private static JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }
}
