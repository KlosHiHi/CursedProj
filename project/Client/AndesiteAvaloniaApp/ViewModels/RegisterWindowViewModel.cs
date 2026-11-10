using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class RegisterWindowViewModel(Action<string> action) : ParentLoginViewModel(action)
    {
        private readonly Action<string> _loginAction = action;


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
            _loginAction?.Invoke("register");
            window.Close();
        }
    }
}
