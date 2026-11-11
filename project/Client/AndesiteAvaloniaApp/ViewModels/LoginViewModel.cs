using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class LoginViewModel(Window window, Action<string> action) : ViewModelBase
    {
        private readonly Window _mainWindow = window;

        [RelayCommand]
        private async Task OpenWindow(string name)
        {
            var windowPathString = $"AndesiteAvaloniaApp.Views.{name}Window";
            var viewModelPathString = $"AndesiteAvaloniaApp.ViewModels.{name}WindowViewModel";

            var windowType = Type.GetType(windowPathString);
            var viewModelType = Type.GetType(viewModelPathString);

            if (windowType is null || viewModelType is null)
                return;

            if (Activator.CreateInstance(windowType) is Window window)
            {
                var viewModel = Activator.CreateInstance(viewModelType, action) as ParentLoginViewModel;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.DataContext = viewModel;

                await window.ShowDialog(_mainWindow);
            }
        }
    }
}