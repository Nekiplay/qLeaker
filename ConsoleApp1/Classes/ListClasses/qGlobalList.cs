using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace CloudLibrary
{
    public class qGlobalList
    {
        public List<string> lists = new List<string>();

        public async Task<List<qList>> LoadFromCloudAsync()
        {
            List<qList> loaded = new List<qList>();
            List<Task> tasks = new List<Task>();

            HttpClient httpClient = new HttpClient();
            foreach (string url in lists)
            {
                Task<qList> temp = new Task<qList>(() =>
                {
                    string list_json = httpClient.GetStringAsync(new Uri(url)).Result;
                    qList list = qList.FromJson(list_json);
                    return list;
                });
                temp.Start();
                tasks.Add(temp);
            }
            
            foreach (Task<qList> list_task in tasks)
            {
                qList list = await list_task;
                loaded.Add(list);
            }
            return loaded;
        }

        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static qGlobalList FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<qGlobalList>(json, settings);
        }
    }
}
