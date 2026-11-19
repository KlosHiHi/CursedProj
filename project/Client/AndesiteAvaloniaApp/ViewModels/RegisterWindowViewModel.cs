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
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Минимальная длина логина 4 символа")]
        private string? _login;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Пароль обязательно ввести")]
        [RegularExpression("(?=.*\\d)(?=.*[@%&$\\-_!#^*+=])(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "Пароль слишком простой")]
        private string? _password;

        [ObservableProperty]
        [Required(ErrorMessage = "Повторите пароль")]
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

            var checkLoginResponse = await _authservice.CheckLogin(Login!);

            if (!checkLoginResponse)
            {
                await MessageBoxService.ShowSimpleMessageBox("Указанный логин занят", MsgBoxType.Error);
                return;
            }

            var response = await _authservice.RegisterUserAsync(Login!, Password!, Email);

            if (response is null)
            {
                await MessageBoxService.ShowSimpleMessageBox("Введённые данные не верны", MsgBoxType.Error);
                return;
            }
            else
            {
                await MessageBoxService.ShowSimpleMessageBox("Вы успешно зарегистрированы", MsgBoxType.Success);
            }

            var token = await _authservice.AuthUserAsync(Login!, Password!);

            if (String.IsNullOrEmpty(token))
            {
                await MessageBoxService.ShowSimpleMessageBox("При авторизации произошла ошибка", MsgBoxType.Error);
                return;
            }
            else
            {
                TokenStorage.Instance.Token = token;
                _authservice.CompleteWork(window, "register");
            }
        }

        private async Task<bool> IsValidData()
        {
            string login = Login ?? "";
            string password = Password ?? "";
            string checkPassword = CheckPassword ?? "";

            if (String.IsNullOrEmpty(login))
            {
                await MessageBoxService.ShowSimpleMessageBox("Поле логина не должно быть пустым", MsgBoxType.Info);
                return false;
            }

            if (String.IsNullOrEmpty(password))
            {
                await MessageBoxService.ShowSimpleMessageBox("Поле пароль не должно быть пустым", MsgBoxType.Info);
                return false;
            }

            if (password != checkPassword)
            {
                await MessageBoxService.ShowSimpleMessageBox("Пароли не совпадают", MsgBoxType.Info);
                return false;
            }

            return true;
        }
    }
}
