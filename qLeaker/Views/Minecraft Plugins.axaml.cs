using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using CloudLibrary;
using Splat;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace qLeaker.Views
{
    public partial class Minecraft_Plugins : Window
    {
        int row = 0;
        int colum = 0;
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
        public void RemoveText(object? sender, GotFocusEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (instance.Text == instance.Tag.ToString())
                instance.Text = "";
        }
        public void AddText(object? sender, RoutedEventArgs e)
        {
            TextBox instance = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(instance.Text))
                instance.Text = instance.Tag.ToString();
        }
        public async void Back(object sender, RoutedEventArgs args)
        {
            this.Hide();
            App.gameSelector.Show();
        }
        public void CategoryClick(object? sender, RoutedEventArgs args)
        {
            UpdateSearch();
        }
        public void UpdateCategory(MinecraftPlugin addon)
        {
            if (AnticheatCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(MinecraftPlugin.Type.Anticheat))
            { 
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (MinigameCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(MinecraftPlugin.Type.Minigame))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (MiscCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(MinecraftPlugin.Type.Misc))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (GUICheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(MinecraftPlugin.Type.GUI))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (PvPCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(MinecraftPlugin.Type.PvP))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (PvECheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(MinecraftPlugin.Type.PvE))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (
                ( AnticheatCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(MinecraftPlugin.Type.Anticheat) )
                && (MinigameCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(MinecraftPlugin.Type.Minigame))
                && (MiscCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(MinecraftPlugin.Type.Misc))
                && (GUICheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(MinecraftPlugin.Type.GUI))
                && (PvPCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(MinecraftPlugin.Type.PvP))
                && (PvPCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(MinecraftPlugin.Type.PvE))
            )
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
        }
        public void UpdateSearch()
        {
            row = 0;
            colum = 0;
            Dispatcher.UIThread.Post(() =>
            {
                Items.Children.Clear();
            }, DispatcherPriority.Background);
            string text = SearchBox.Text;
            if (string.IsNullOrWhiteSpace(text) || text == "")
            {
                text = "";
            }
            new Thread(async () =>
            {
                foreach (var list in App.gameSelector.qLists)
                {
                    foreach (MinecraftPlugin addon in list.mc_plugins)
                    {
                        if (text != "Search" && text != "")
                        {
                            if (addon.name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                UpdateCategory(addon);
                            }
                        }
                        else
                        {
                            UpdateCategory(addon);
                        }
                    }
                }
            }).Start();
        }
        public void SearchInput(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                UpdateSearch();
            }
        }
        public Minecraft_Plugins()
        {
            InitializeComponent();
        }
        public void LoadAll()
        {
            foreach (var list in App.gameSelector.qLists)
            {
                foreach (MinecraftPlugin addon in list.mc_plugins)
                {
                    Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
                }
            }
        }
        public Bitmap ImageFromUrl(byte[] url)
        {
            using (WebClient wc = new WebClient())
            {
                MemoryStream stream = new MemoryStream(url);
                Bitmap Bitmap = new Bitmap(stream);
                return Bitmap;
            }
        }
        public void Add(MinecraftPlugin plugin)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Avalonia.Thickness(5, 5, 0, 0);
            stackPanel.Height = 95;
            Grid.SetColumn(stackPanel, colum);
            Grid.SetRow(stackPanel, row);

            colum++;
            if (colum >= 4)
            {
                row++;
                colum = 0;
                Items.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            }

            Border border = new Border();
            border.CornerRadius = new CornerRadius(8, 8, 8, 8);
            border.Width = 280;
            border.Height = 95;
            border.Opacity = 120;
            border.Background = new SolidColorBrush()
            {
                Color = Colors.Black
            };

            Border border2 = new Border();
            border2.Margin = new Avalonia.Thickness(5, -95 + 5, 0, 0);
            border2.CornerRadius = new CornerRadius(8, 8, 8, 8);
            border2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            border2.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            border2.Width = 48;
            border2.Height = 48;

            border2.Background = new ImageBrush()
            {
                Stretch = Stretch.Fill,
                Source = ImageFromUrl(plugin.image)
            };

            TextBlock text = new TextBlock();
            text.Text = plugin.name + " v" + plugin.version;
            text.Margin = new Avalonia.Thickness(60, -118 + 30, 0, 0);

            TextBlock text2 = new TextBlock();
            text2.Text = plugin.mcversion;
            text2.Margin = new Avalonia.Thickness(60, -96 + 30, 0, 0);
            text2.FontSize = 10;

            Button button3 = new Button();
            button3.FontSize = 12;
            button3.Margin = new Avalonia.Thickness(140, -88 + 45, 0, 0);
            button3.Height = 28;
            button3.Width = 57;
            button3.Content = "Source";
            button3.Background = new SolidColorBrush(Color.Parse("#2882b5"));
            button3.Click += async (s, e) =>
            {
                await Task.Factory.StartNew(() =>
                {
                    ByteCryptor byteCryptor = new ByteCryptor("BrokenCore");
                    using (WebClient wc = new WebClient())
                    {
                        Directory.CreateDirectory("Download");
                        Directory.CreateDirectory("Download\\MC Source");

                        byte[] dw = wc.DownloadData(plugin.source);
                        byte[] decrypted = byteCryptor.Decrypt(dw);
                        string file_path = "Download\\MC Source\\" + plugin.name + " v" + plugin.version + ".zip";
                        File.Create(file_path).Close();
                        File.WriteAllBytes(file_path, decrypted);
                    }
                });
            };

            Button button2 = new Button();
            button2.FontSize = 12;
            button2.Margin = new Avalonia.Thickness(5, -88 + 45, 0, 0);
            button2.Height = 28;
            button2.Width = 9 * plugin.store.name.Length + 2;
            button2.Content = plugin.store.name;
            button2.Background = new SolidColorBrush(Color.Parse("#2882b5"));
            button2.Click += (s, e) =>
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = plugin.store.url,
                    UseShellExecute = true
                });
            };

            Button button = new Button();
            button.FontSize = 12;
            button.Margin = new Avalonia.Thickness(200, -88 + 45, 0, 0);
            button.Height = 28;
            button.Width = 75;
            button.Content = "Download";
            button.Background = new SolidColorBrush(Color.Parse("#2882b5"));
            button.Click += async (s, e) =>
            {
                await Task.Factory.StartNew(() =>
                {
                    ByteCryptor byteCryptor = new ByteCryptor("BrokenCore");
                    using (WebClient wc = new WebClient())
                    {
                        Directory.CreateDirectory("Download");
                        Directory.CreateDirectory("Download\\MC");

                        byte[] dw = wc.DownloadData(plugin.download);
                        byte[] decrypted = byteCryptor.Decrypt(dw);
                        string file_path = "Download\\MC\\" + plugin.version + " v" + plugin.version + plugin.extension;
                        File.Create(file_path).Close();
                        File.WriteAllBytes(file_path, decrypted);
                    }
                });
            };

            stackPanel.Children.Add(border);
            stackPanel.Children.Add(border2);
            stackPanel.Children.Add(text2);
            stackPanel.Children.Add(text);
            if (plugin.source != "")
            {
                stackPanel.Children.Add(button3);
            }
            if (plugin.store.url != "")
            {
                stackPanel.Children.Add(button2);
            }
            stackPanel.Children.Add(button);
            this.FindControl<Grid>("Items").Children.Add(stackPanel);
            //Items.Children.Add(stackPanel);
        }
    }
}
