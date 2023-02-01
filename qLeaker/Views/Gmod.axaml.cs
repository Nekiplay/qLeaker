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
    public partial class Gmod : Window
    {
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
        public void Back(object sender, RoutedEventArgs args)
        {
            this.Hide();
            App.gameSelector.Show();
        }
        public void CategoryClick(object sender, RoutedEventArgs args)
        {
            UpdateSearch();
        }
        public void SearchInput(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (e.Key == Avalonia.Input.Key.Enter)
            {
                UpdateSearch();
            }
        }

        public void UpdateCategory(GmodAddon addon)
        {
            if (HudCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Hud))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (F4CheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.F4))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (ScoreboardCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Scoreboard))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (LogsCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Logs))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (AdminCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Admin))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (EntityCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Entity))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (PrinterCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Printer))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (InventoryCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Inventory))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (GuiCheckBox.IsChecked.GetValueOrDefault(false) == true && addon.types.Contains(GmodAddon.Type.Gui))
            {
                Dispatcher.UIThread.Post(() => Add(addon), DispatcherPriority.Background);
            }
            else if (
                (HudCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Hud))
                && (F4CheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.F4))
                && (ScoreboardCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Scoreboard))
                && (LogsCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Logs))
                && (EntityCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Entity))
                && (AdminCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Admin))
                && (PrinterCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Printer))
                && (InventoryCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Inventory))
                && (GuiCheckBox.IsChecked.GetValueOrDefault(false) == false && !addon.types.Contains(GmodAddon.Type.Gui))
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
                    foreach (GmodAddon addon in list.gmod_addons)
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
        public Gmod()
        {
            this.Closing += (o, c) =>
            {
                Process.GetCurrentProcess().Kill();
            };
            InitializeComponent();
        }

        public void LoadAll()
        {
            foreach (var list in App.gameSelector.qLists)
            {
                foreach (GmodAddon addon in list.gmod_addons)
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
        int row = 0;
        int colum = 0;
        public void Add(GmodAddon addon)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Avalonia.Thickness(5, 5, 0, 0);
            stackPanel.Height = 190;
            Grid.SetColumn(stackPanel, colum);
            Grid.SetRow(stackPanel, row);

            colum++;
            if (colum >= 4)
            {
                row++;
                colum = 0;
                Items.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                //Items.Ro.Add(new RowDefinition());
            }

            Border border = new Border();
            border.CornerRadius = new CornerRadius(8, 8, 0, 0);
            border.Width = 280;
            border.Height = 120;

            border.Background = new ImageBrush()
            {
                Stretch = Stretch.Fill,
                Source = ImageFromUrl(addon.image)
            };

            //Image image = new Image();
            //image.Stretch = Avalonia.Media.Stretch.Fill;
            //image.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            //image.Width = 280;
            //image.Height = 120;
            //image.Source = ImageFromUrl(addon.Logo);

            Border border2 = new Border();
            border2.CornerRadius = new CornerRadius(0, 0, 8, 8);
            border2.Width = 280;
            border2.Height = 70;
            border2.Opacity = 120;
            border2.Background = new SolidColorBrush()
            {
                Color = Colors.Black
            };

            //StackPanel stackPanel2 = new StackPanel();
            //stackPanel2.Opacity = 120;
            //stackPanel2.Height = 70;
            //stackPanel2.Width = 280;
            //stackPanel2.Background = new SolidColorBrush(Colors.Black);
            //stackPanel2.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;

            TextBlock textBlock = new TextBlock();
            textBlock.Text = addon.name;
            textBlock.FontSize = 12;
            textBlock.Margin = new Avalonia.Thickness(10, -60, 0, 0);

            Button button2 = new Button();
            button2.FontSize = 12;
            button2.Margin = new Avalonia.Thickness(7, -45, 0, 0);
            button2.Height = 28;
            button2.Width = 9 * addon.store.name.Length;
            button2.Content = addon.store.name;
            button2.Background = new SolidColorBrush(Color.Parse("#2882b5"));
            button2.Click += (s, e) =>
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = addon.store.url,
                    UseShellExecute = true
                });
            };

            Button button3 = new Button();
            button3.FontSize = 12;
            button3.Margin = new Avalonia.Thickness(130, -45, 0, 0);
            button3.Height = 28;
            button3.Width = 65;
            button3.Content = "Content";
            button3.Background = new SolidColorBrush(Color.Parse("#2882b5"));
            button3.Click += (s, e) =>
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = addon.content,
                    UseShellExecute = true
                });
            };

            Button button = new Button();
            button.FontSize = 12;
            button.Margin = new Avalonia.Thickness(200, -45, 0, 0);
            button.Height = 28;
            button.Width = 74;
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
                        Directory.CreateDirectory("Download\\Gmod");

                        byte[] dw = wc.DownloadData(addon.download);
                        byte[] decrypted = byteCryptor.Decrypt(dw);
                        string file_path = "Download\\Gmod\\" + addon.name + ".zip";
                        File.Create(file_path).Close();
                        File.WriteAllBytes(file_path, decrypted);
                    }
                });
            };

            stackPanel.Children.Add(border);
            stackPanel.Children.Add(border2);
            stackPanel.Children.Add(textBlock);
            if (addon.store.name != "")
            {
                stackPanel.Children.Add(button2);
            }
            if (addon.content != "")
            {
                stackPanel.Children.Add(button3);
            }
            stackPanel.Children.Add(button);
            this.FindControl<Grid>("Items").Children.Add(stackPanel);
            //Items.Children.Add(stackPanel);
        }
    }
}
