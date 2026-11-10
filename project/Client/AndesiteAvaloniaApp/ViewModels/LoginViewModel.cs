using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class LoginViewModel(Window window) : ViewModelBase
    {
        private readonly Window _mainWindow = window;

        [RelayCommand]
        private async Task OpenWindow(string name)
        {
            var pathString = $"AndesiteAvaloniaApp.Views.{name}Window";
            var windowType = Type.GetType(pathString);

            if (windowType is null)
                return;

            if (Activator.CreateInstance(windowType) is Window window)
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                var result = window.ShowDialog(_mainWindow);
            }
        }
    }
}
