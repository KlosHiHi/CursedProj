using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class SignInWindowViewModel(IAuthService authService) : ViewModelBase
    {
        private readonly IAuthService _authservice = authService;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Логин обязательно ввести")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина от 4 до 50 символов")]
        private string? _login;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Пароль обязательно ввести")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина от 4 до 50 символов")]
        private string? _password;

        [RelayCommand]
        private async Task SignIn(Window window)
        {
            if (!await IsValidData())
                return;

            var response = await _authservice.AuthUserAsync(Login!, Password!);

            if (response is null)
            {
                await MessageBoxService.ShowError();
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

            return true;
        }
    }
}
