using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class ProfileViewModel(Action action) : ViewModelBase
    {
        private readonly Action _logoutAction = action;


        [RelayCommand]
        private void LogOut()
        {
            _logoutAction?.Invoke();
        }
    }
}
