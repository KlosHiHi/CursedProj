using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;
        private readonly LoginViewModel _loginView;
        public event Action<string>? OnLoginSuccess;

        [ObservableProperty]
        private ViewModelBase _currentPage;

        public MainWindowViewModel(Window window)
        {
            OnLoginSuccess += MainWindowViewModel_OnLoginSuccess;
            _mainWindow = window;

            if (true)
            {
                CurrentPage = new MenuViewModel(window);
            }
            else
            {
                _loginView = new LoginViewModel(_mainWindow, OnLoginSuccess);
                CurrentPage = _loginView;
            }
        }

        private void MainWindowViewModel_OnLoginSuccess(string result)
        {
            CurrentPage = new MenuViewModel(_mainWindow);
        }
    }
}
