using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class RegisterWindowViewModel(IAuthService authService) : ViewModelBase
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
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Длина от 4 до 20 символов")]
        private string? _password;

        [ObservableProperty]
        private string? _checkPassword;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression("([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)", ErrorMessage = "Некорректный формат")]
        private string? _email;


        [RelayCommand]
        private async Task Register(Window window)
        {
            if (!await IsValidData())
                return;

            var response = await _authservice.RegisterUserAsync(Login!, Password!, Email);

            if (response is null)
            {
                await MessageBoxService.ShowError();
            }
            else
            {

            }

            var token = await _authservice.AuthUserAsync(Login!, Password!);

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
            string checkPassword = CheckPassword ?? "";

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

            if (password != checkPassword)
            {
                await MessageBoxService.ShowInfo();
                return false;
            }

            return true;
        }
    }
}
