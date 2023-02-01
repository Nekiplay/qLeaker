using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CloudLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace qLeaker.Views
{
    public partial class GameSelector : Window
    {
        public Gmod gmod = new Gmod();
        public Minecraft_Plugins mc_plugins = new Minecraft_Plugins();
        public void MoveForm(object? sender, PointerPressedEventArgs e)
        {
            if (e.MouseButton == Avalonia.Input.MouseButton.Left)
            {
                this.BeginMoveDrag(e);
            }
        }
        public async void Close(object sender, RoutedEventArgs args)
        {
            Process.GetCurrentProcess().Kill();
        }
        public void OpenGmod(object sender, PointerPressedEventArgs args)
        {
            if (gmod != null)
            {
                this.Hide();
                gmod.Show();
            }
        }
        public void OpenMinecraftPlugins(object sender, PointerPressedEventArgs args)
        {
            if (mc_plugins != null)
            {
                this.Hide();
                mc_plugins.Show();
            }
        }

        public qGlobalList qGlobal = new qGlobalList();
        public List<qList> qLists = new List<qList>();
        public GameSelector()
        {
            this.Closing += (o, c) =>
            {
                Process.GetCurrentProcess().Kill();
            };
            this.Initialized += (o, i) =>
            {
                qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/1.json");
                qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/2.json");
                qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/3.json");
                qGlobal.lists.Add("https://github.com/Nekiplay/qLeaker-Cloud/raw/main/qList/4.json");

                Thread loader = new Thread(async() =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    qLists = await qGlobal.LoadFromCloudAsync();
                    stopwatch.Stop();
                    long mils = stopwatch.ElapsedMilliseconds;
                    Debug.WriteLine("DataBase loaded in milliseconds: " + mils);

                    Thread gmod_loader = new Thread(() =>
                    {
                        int c = 0;

                        foreach (var l in qLists)
                        {
                            c += l.gmod_addons.Count;
                        }
                        Dispatcher.UIThread.Post(() => { Gmod_Addons.Text = "Garry's mod addons [" + c + "]"; }, DispatcherPriority.Background);
                        gmod.LoadAll();
                    });
                    gmod_loader.Start();

                    Thread minecraft_plugins_loader = new Thread(() =>
                    {
                        int c = 0;

                        foreach (var l in qLists)
                        {
                            c += l.mc_plugins.Count;
                        }
                        Dispatcher.UIThread.Post(() => { mc_plugins_text.Text = "Spigot plugins [" + c + "]"; }, DispatcherPriority.Background);
                        mc_plugins.LoadAll();
                    });
                    minecraft_plugins_loader.Start();
                });
                loader.Start();
            };
            InitializeComponent();
        }
    }
}
