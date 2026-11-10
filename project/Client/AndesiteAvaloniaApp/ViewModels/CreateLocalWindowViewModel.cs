using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class CreateLocalWindowViewModel(Action<string> action) : ParentLoginViewModel(action)
    {
        [ObservableProperty]
        private string? _localName;

        private readonly Action<string> _loginAction = action;

        [RelayCommand]
        private void CreateLocal(Window window)
        {
            _loginAction?.Invoke("create_local");
            window.Close();
        }
    }
}
