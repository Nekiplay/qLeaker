using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using qLeaker.ViewModels;
using qLeaker.Views;

namespace qLeaker
{
    public partial class App : Application
    {
        public static GameSelector gameSelector = new GameSelector();
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = gameSelector;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
