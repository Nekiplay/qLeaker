using CloudLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static qList qList1 = new qList();
        static qList qList2 = new qList();
        static qList qList3 = new qList();
        static qList qList4 = new qList();

        static List<qList> lists = new List<qList>()
        {
            new qList(),
            new qList(),
            new qList(),
            new qList()
        };
        static qGlobalList qGlobal = new qGlobalList();
        static void Main(string[] args)
        {
            Directory.CreateDirectory("qLists");
            Directory.CreateDirectory("qLists\\lists");
            Directory.CreateDirectory("qLists\\lists");

            #region Загрузка локальной базы данных
            for (int i = 1; i < lists.Count + 1; i++)
            {
                if (File.Exists("qLists\\lists\\" + i + ".json"))
                {
                    string json = File.ReadAllText("qLists\\lists\\" + i + ".json");
                    lists[i - 1] = qList.FromJson(json);
                }
            }
            int total_mc_plugins = 0;
            foreach (qList q in lists)
            {
                total_mc_plugins += q.mc_plugins.Count;
            }
            #endregion

            int mc_plugin_slot = 0;
            int gmod_addon_slot = 0;

            #region Получение Gmod addon слота
            int gmod_qc1 = lists[0].gmod_addons.Count;
            int gmod_qc2 = lists[1].gmod_addons.Count;
            int gmod_qc3 = lists[2].gmod_addons.Count;
            int gmod_qc4 = lists[3].gmod_addons.Count;

            int[] gmod_addon_array = new int[] { gmod_qc1, gmod_qc2, gmod_qc3, gmod_qc4 };

            int gmod_addon_min = gmod_addon_array.Min();
            if (gmod_addon_min == gmod_qc1)
            {
                gmod_addon_slot = 1;
            }
            else if (gmod_addon_min == gmod_qc2)
            {
                gmod_addon_slot = 2;
            }
            else if (gmod_addon_min == gmod_qc3)
            {
                gmod_addon_slot = 3;
            }
            else if (gmod_addon_min == gmod_qc4)
            {
                gmod_addon_slot = 4;
            }
            #endregion

            #region Получение Minecraft Plugin слота
            int mc_qc1 = lists[0].mc_plugins.Count;
            int mc_qc2 = lists[1].mc_plugins.Count;
            int mc_qc3 = lists[2].mc_plugins.Count;
            int mc_qc4 = lists[3].mc_plugins.Count;

            int[] mc_addon_array = new int[] { mc_qc1, mc_qc2, mc_qc3, mc_qc4 };

            int mc_plugin_min = mc_addon_array.Min();
            if (mc_plugin_min == mc_qc1)
            {
                mc_plugin_slot = 1;
            }
            else if (mc_plugin_min == mc_qc2)
            {
                mc_plugin_slot = 2;
            }
            else if (mc_plugin_min == mc_qc3)
            {
                mc_plugin_slot = 3;
            }
            else if (mc_plugin_min == mc_qc4)
            {
                mc_plugin_slot = 4;
            }
            #endregion

            bool add_new = true;

            qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/1.json");
            qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/2.json");
            qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/3.json");
            qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/4.json");

            Task.Factory.StartNew(async() =>
            {
                var result = await qGlobal.LoadFromCloudAsync();
            });

            string json2 = qGlobal.ToJson();

            bool gmod = false;

            if (add_new)
            {
                if (gmod)
                {
                    GmodAddon addon = new GmodAddon();
                    addon.name = "xInventory - Inventory and Trading System";
                    addon.types = new List<GmodAddon.Type>() { GmodAddon.Type.Inventory };
                    addon.image = ImageTool.ImageFromUrl("https://i.ibb.co/gSTMFtj/3e6e779e4a9f9ca6cb59579bd6c6448d.jpg");
                    addon.store = new Store("Gmodstore", "https://www.gmodstore.com/market/view/xinventory-inventory-and-trading-system-2");
                    addon.content = "https://steamcommunity.com/sharedfiles/filedetails/?id=2061339328";
                    addon.download = "https://github.com/Nekiplay/GmodAddons/raw/main/files/xinventory.zip";

                    bool find = false;
                    foreach (qList qList in lists)
                    {
                        foreach (GmodAddon addon2 in qList.gmod_addons)
                        {
                            if (addon2.name == addon.name)
                            {
                                find = true;
                            }
                        }
                    }

                    if (!find)
                    {
                        Console.WriteLine("Сохраняю addon " + addon.name + " в список: " + gmod_addon_slot + ".json");

                        Save(gmod_addon_slot, addon);
                    }
                }
                else
                {
                    bool find = false;
                    MinecraftPlugin minecraftPlugin = new MinecraftPlugin();
                    minecraftPlugin.name = "cPractice";
                    minecraftPlugin.version = "1.1.4";
                    minecraftPlugin.mcversion = "1.7 - 1.8";
                    minecraftPlugin.extension = ".zip";
                    minecraftPlugin.types = new List<MinecraftPlugin.Type>() { MinecraftPlugin.Type.PvP, MinecraftPlugin.Type.Minigame };
                    minecraftPlugin.store = new Store("Spigot", "");
                    minecraftPlugin.image = ImageTool.ImageFromUrl("https://i.ibb.co/Bcft6Jq/5684-1.png");
                    minecraftPlugin.java = MinecraftPlugin.Java.Any;
                    minecraftPlugin.download = "https://github.com/Nekiplay/MCAddons/raw/main/files/cPractice_1.1.4.zip";

                    foreach (qList qList in lists)
                    {
                        foreach (MinecraftPlugin addon2 in qList.mc_plugins)
                        {
                            if (addon2.name == minecraftPlugin.name && addon2.version == minecraftPlugin.version)
                            {
                                find = true;
                            }
                        }
                    }
                    if (!find)
                    {
                        Console.WriteLine("Сохраняю plugin " + minecraftPlugin.name + " в список: " + mc_plugin_slot + ".json");
                        Save(mc_plugin_slot, minecraftPlugin);
                    }
                    else
                    {
                        Console.WriteLine(minecraftPlugin.name + " v" + minecraftPlugin.version + " уже есть в базе данной");
                    }
                }
            }
            Console.ReadLine();
        }

        public static void Save(int slot, GmodAddon addon)
        {
            lists[slot - 1].gmod_addons.Add(addon);
            File.Create("qLists\\lists\\" + slot + ".json").Close();
            File.WriteAllText("qLists\\lists\\" + slot + ".json", lists[slot -1 ].ToJson());
        }
        public static void Save(int slot, MinecraftPlugin plugin)
        {
            lists[slot - 1].mc_plugins.Add(plugin);
            File.Create("qLists\\lists\\" + slot + ".json").Close();
            File.WriteAllText("qLists\\lists\\" + slot + ".json", lists[slot - 1].ToJson());
        }
    }
    
}
