using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Scalex.UI.ViewModels;
using Scalex.UI.Views;

namespace Scalex.UI
{
    public partial class App : Application
    {
        private IAppSettingsPprovider? _appSettingsProvider;

        public App() : base()
        {

        }

        public App(IAppSettingsPprovider settingsProvider) : base()
        {
            _appSettingsProvider = settingsProvider;
        }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public IAppSettingsPprovider GetSettingsProvider()
        {
            return _appSettingsProvider;
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}