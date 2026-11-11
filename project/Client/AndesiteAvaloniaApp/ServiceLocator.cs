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
                .AddScoped<LoginViewModel>()
                .AddScoped<MenuViewModel>()

                .AddScoped<NoteListViewModel>()
                .AddScoped<ProfileViewModel>()
                ;

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
