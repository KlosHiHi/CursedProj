using AndesiteAvaloniaApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AndesiteAvaloniaApp
{
    public static class ServiceLocator
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static void ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddTransient<MainWindowViewModel>()
                .AddTransient<LoginViewModel>()
                .AddTransient<MenuViewModel>()

                .AddTransient<ParentLoginViewModel>()
                .AddTransient<CreateLocalWindowViewModel>()
                .AddTransient<RegisterWindowViewModel>()
                .AddTransient<SignInWindowViewModel>()
                ;

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
