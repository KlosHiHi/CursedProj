using AndesiteAvaloniaApp.Options;
using AndesiteAvaloniaApp.Services;
using AndesiteAvaloniaApp.Views;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private LoginViewModel _loginView = null!;
        private MenuViewModel _menuView = null!;
        public event Action<string>? OnLoginSuccess;
        public event Action? OnLogout;

        [ObservableProperty]
        private ViewModelBase? _currentPage;

        public MainWindowViewModel()
        {
            Configuration();
            StartSession();
        }

        private async void StartSession()
        {
            if (await CheckAuth())
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

        private static async Task<bool> CheckAuth()
        {
            var jwt = TokenStorage.Instance.Token;

            if (String.IsNullOrEmpty(jwt))
                return false;

            var responce = await HttpClientOption.Client.GetAsync($"{HttpClientOption.BaseUrl}Login/CheckToken");

            return responce.StatusCode switch
            {
                HttpStatusCode.Unauthorized => false,
                HttpStatusCode.NoContent => true,
                _ => false
            };
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
