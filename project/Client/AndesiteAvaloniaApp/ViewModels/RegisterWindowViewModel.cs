using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        private void Register(Window window)
        {
            _authservice.CompleteWork(window, "register");
        }
    }
}
