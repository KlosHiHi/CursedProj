using AndesiteAvaloniaApp.Services;
using AndesiteAvaloniaApp.ViewModels;
using AndesiteAvaloniaApp.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace AndesiteAvaloniaApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            ServiceLocator.ConfigureServices();
            TokenStorage.Instance.Load();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = new MainWindow();

                var authService = ServiceLocator.ServiceProvider.GetRequiredService<IAuthService>();
                authService.MainWindow = mainWindow;

                mainWindow.DataContext = new MainWindowViewModel();

                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}