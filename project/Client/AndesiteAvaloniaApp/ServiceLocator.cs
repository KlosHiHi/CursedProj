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

                .AddTransient<SignInWindowViewModel>()
                .AddTransient<RegisterWindowViewModel>()
                .AddTransient<CreateLocalWindowViewModel>()

                .AddScoped<NoteListViewModel>()
                .AddScoped<ProfileViewModel>()
                .AddScoped<ArchiveViewModel>()
                .AddScoped<TrashViewModel>()
                .AddScoped<SettingViewModel>()

                .AddScoped<NoteRedactorViewModel>()

                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IEditProfileService, EditProfileService>()
                .AddScoped<INoteEditorService, NoteEditorService>()
                ;

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
