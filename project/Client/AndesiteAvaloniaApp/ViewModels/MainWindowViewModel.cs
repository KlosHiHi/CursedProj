using AndesiteAvaloniaApp.Views;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;
        private readonly LoginViewModel _loginView;
        private readonly MenuViewModel _menuView;
        public event Action<string>? OnLoginSuccess;
        public event Action OnLogout;

        [ObservableProperty]
        private ViewModelBase _currentPage;

        public MainWindowViewModel(Window window)
        {
            OnLoginSuccess += MainWindowViewModel_OnLoginSuccess;
            OnLogout += MainWindowViewModel_OnLogout;
            _mainWindow = window;

            if (true)
            {
                _menuView = new MenuViewModel(_mainWindow, OnLogout);
                CurrentPage = _menuView;
            }
            else
            {
                _loginView = new LoginViewModel(_mainWindow, OnLoginSuccess);
                CurrentPage = _loginView;
            }
        }

        private void MainWindowViewModel_OnLogout()
        {
            CurrentPage = _loginView;
        }

        private void MainWindowViewModel_OnLoginSuccess(string result)
        {
            CurrentPage = _menuView;
        }
    }
}
