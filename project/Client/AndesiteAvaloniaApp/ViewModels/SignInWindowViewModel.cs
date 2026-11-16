using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        private void SignIn(Window window)
        {
            _authservice.CompleteWork(window, "sign_in");
        }
    }
}
