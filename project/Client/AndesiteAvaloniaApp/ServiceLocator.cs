using AndesiteAvaloniaApp.Services;
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

                .AddScoped<SignInWindowViewModel>()
                .AddScoped<RegisterWindowViewModel>()
                .AddScoped<CreateLocalWindowViewModel>()

                .AddScoped<NoteListViewModel>()
                .AddScoped<ProfileViewModel>()

                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IEditProfileService, EditProfileService>()
                ;

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
