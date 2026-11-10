using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Window _mainWindow;

        [ObservableProperty]
        private ViewModelBase _currentPage;

        public MainWindowViewModel(Window window)
        {
            _mainWindow = window;

            if (false)
            {
                CurrentPage = new MenuViewModel();
            }
            else
            {
                CurrentPage = new LoginViewModel(_mainWindow);
            }
        }
    }
}
