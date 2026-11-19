using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class SignInWindowViewModel(IAuthService authService) : ViewModelBase
    {
        private readonly IAuthService _authservice = authService;

        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [RelayCommand]
        private async Task SignIn(Window window)
        {
            if (!await IsValidData())
                return;

            var response = await _authservice.AuthUserAsync(Login!, Password!);

            if (response is null)
            {
                await MessageBoxService.ShowSimpleMessageBox("Введённые данные не верны", MsgBoxType.Error);
            }
            else
            {
                TokenStorage.Instance.Token = response;
                _authservice.CompleteWork(window, "sign_in");
            }
        }

        private async Task<bool> IsValidData()
        {
            string login = Login ?? "";
            string password = Password ?? "";

            if (String.IsNullOrWhiteSpace(login))
            {
                await MessageBoxService.ShowSimpleMessageBox("Поле логина не должно быть пустым", MsgBoxType.Info);
                return false;
            }

            if (String.IsNullOrWhiteSpace(password))
            {
                await MessageBoxService.ShowSimpleMessageBox("Поле пароль не должно быть пустым", MsgBoxType.Info);
                return false;
            }

            return true;
        }
    }
}
