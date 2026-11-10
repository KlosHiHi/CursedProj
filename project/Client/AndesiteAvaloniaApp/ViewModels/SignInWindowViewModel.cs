using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class SignInWindowViewModel(Action<string> action) : ParentLoginViewModel(action)
    {
        private readonly Action<string> _loginAction = action;

        [ObservableProperty]
        private string? _login;

        [ObservableProperty]
        private string? _password;

        [RelayCommand]
        private void SignIn(Window window)
        {
            _loginAction?.Invoke("sign_in");
            window.Close();
        }
    }
}
