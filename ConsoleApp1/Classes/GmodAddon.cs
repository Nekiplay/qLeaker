using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace CloudLibrary
{
    public class GmodAddon
    {
        public string name = "";
        public byte[] image = new byte[] { };
        public Store store = new Store("", "");
        public string content = "";
        public string download = "";

        public List<Type> types = new List<Type>();

        public enum Type
        {
            None,
            Hud,
            F4,
            Entity,
            Scoreboard,
            Inventory,
            Printer,
            Admin,
            Logs,
            Gui,
        }

        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static GmodAddon FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<GmodAddon>(json, settings);
        }
    }
}
