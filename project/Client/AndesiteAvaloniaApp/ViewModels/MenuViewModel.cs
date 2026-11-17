using AndesiteAvaloniaApp.Helpers;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml.Linq;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MenuViewModel() : ViewModelBase
    {
        private static readonly int _menuPanelOpenWidth = 350;
        private static readonly int _menuPanelCloseWidth = 60;
        private static readonly Bitmap? _openImage = ImageHelper.LoadFromResource(new Uri("avares://AndesiteAvaloniaApp/Assets/Icons/andesite-start.png"));
        private static readonly Bitmap _closeImage = ImageHelper.LoadFromResource(new Uri("avares://AndesiteAvaloniaApp/Assets/Icons/andesite-finish.png"));

        [ObservableProperty]
        private static bool _isOpen = false;

        [ObservableProperty]
        private ViewModelBase _workPage = ServiceLocator.ServiceProvider.GetRequiredService<NoteListViewModel>();

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

        [RelayCommand]
        private void OpenProfile()
        {
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<ProfileViewModel>();
        }

        [RelayCommand]
        private void OpenSetting()
        {
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<SettingViewModel>();
        }

        [RelayCommand]
        private void OpenArchive()
        {
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<ArchiveViewModel>();
        }

        [RelayCommand]
        private void OpenTrash()
        {
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<TrashViewModel>();
        }

        [RelayCommand]
        private void LoadMenuView()
        {
            MenuImage = _closeImage;
            MenuPanelWidth = _menuPanelCloseWidth;
            IsOpen = false;
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<NoteListViewModel>();
        }
    }
}
