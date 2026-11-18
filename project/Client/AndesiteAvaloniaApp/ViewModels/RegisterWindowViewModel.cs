using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class RegisterWindowViewModel(IAuthService authService) : ViewModelBase
    {
        private readonly IAuthService _authservice = authService;

        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _checkPassword;

        [ObservableProperty]
        private string? _email;


        [RelayCommand]
        private async Task Register(Window window)
        {
            if (!await IsValidData())
                return;

            var response = await _authservice.RegisterUserAsync(Login, Password, Email);

            if (response is null)
            {
                await MessageBoxService.ShowError();
            }
            else
            {

            }

            var token = await _authservice.AuthUserAsync(Login, Password);

            if (token is null)
            {
                await MessageBoxService.ShowError();
            }
            else
            {
                TokenStorage.Instance.Token = token;
                _authservice.CompleteWork(window, "sign_in");
            }

            _authservice.CompleteWork(window, "register");
        }
        private async Task<bool> IsValidData()
        {
            string login = Login ?? "";
            string password = Password ?? "";
            string checkpassword = CheckPassword ?? "";

            if (String.IsNullOrEmpty(login))
            {
                await MessageBoxService.ShowInfo();
                return false;
            }

            if (String.IsNullOrEmpty(password))
            {
                await MessageBoxService.ShowInfo();
                return false;
            }

            if (password != checkpassword)
            {
                await MessageBoxService.ShowInfo();
                return false;
            }

            return true;
        }
    }
}
