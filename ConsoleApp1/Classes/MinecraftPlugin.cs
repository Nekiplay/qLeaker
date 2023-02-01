using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudLibrary
{
    public class MinecraftPlugin
    {
        public string name = "";
        public string version = "";
        public byte[] image = new byte[] { };
        public Store store = new Store("", "");
        public string source = "";
        public string download = "";
        public string extension = "";
        public string mcversion = "";

        public Java java = Java.Any;

        public enum Java
        {
            Any,
            HotSpot,
        }
        public List<Type> types = new List<Type>();
        public enum Type
        {
            None,
            Anticheat,
            Itemframe,
            Food,
            PvP,
            PvE,
            Minigame,
            Misc,
            GUI,
            Shop,
        }

        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, settings);
        }

        public static MinecraftPlugin FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<MinecraftPlugin>(json, settings);
        }
    }
}
