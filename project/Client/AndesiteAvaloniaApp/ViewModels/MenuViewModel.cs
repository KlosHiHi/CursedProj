using AndesiteAvaloniaApp.Helpers;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MenuViewModel(Window window) : ViewModelBase
    {
        private static readonly int _menuPanelOpenWidth = 350;
        private static readonly int _menuPanelCloseWidth = 60;
        private static readonly Bitmap? _openImage = ImageHelper.LoadFromResource(new Uri("avares://AndesiteAvaloniaApp/Assets/Icons/andesite-start.png"));
        private static readonly Bitmap _closeImage = ImageHelper.LoadFromResource(new Uri("avares://AndesiteAvaloniaApp/Assets/Icons/andesite-finish.png"));
        private readonly Window _mainWindow = window;

        [ObservableProperty]
        private static bool _isOpen = false;

        [ObservableProperty]
        private ViewModelBase _workPage;

        [ObservableProperty]
        private int _menuPanelWidth = _menuPanelCloseWidth;

        [ObservableProperty]
        private Bitmap? _menuImage = _closeImage;

        [RelayCommand]
        private void ChangeMenuState()
        {
            MenuImage = IsOpen ? _closeImage : _openImage;
            MenuPanelWidth = IsOpen ? _menuPanelCloseWidth : _menuPanelOpenWidth;

            IsOpen = !IsOpen;
        }
    }
}
