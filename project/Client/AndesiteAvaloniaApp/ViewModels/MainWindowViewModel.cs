using AndesiteAvaloniaApp.Services;
using AndesiteAvaloniaApp.Views;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;
        private LoginViewModel _loginView = null!;
        private MenuViewModel _menuView = null!;
        public event Action<string>? OnLoginSuccess;
        public event Action OnLogout;

        [ObservableProperty]
        private ViewModelBase _currentPage;

        public MainWindowViewModel()
        {
            Configuration();
            StartSession();
        }

        private void StartSession()
        {
            if (true)
            {
                CurrentPage = _menuView;
            }
            else
            {
                CurrentPage = _loginView;
            }
        }

        private void Configuration()
        {
            OnLoginSuccess += MainWindowViewModel_OnLoginSuccess;
            OnLogout += MainWindowViewModel_OnLogout;

            var authService = ServiceLocator.ServiceProvider.GetRequiredService<IAuthService>();
            authService.LoginAction = OnLoginSuccess;

            var profileService = ServiceLocator.ServiceProvider.GetRequiredService<IEditProfileService>();
            profileService.LogoutAction = OnLogout;

            _menuView = ServiceLocator.ServiceProvider.GetRequiredService<MenuViewModel>();
            _loginView = ServiceLocator.ServiceProvider.GetRequiredService<LoginViewModel>();
        }

        private void MainWindowViewModel_OnLogout()
        {
            CurrentPage = _loginView;
        }

        private void MainWindowViewModel_OnLoginSuccess(string result)
        {
            CurrentPage = _menuView;
        }
    }
}
