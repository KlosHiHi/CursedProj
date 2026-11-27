using AndesiteAvaloniaApp.Helpers;
using AndesiteAvaloniaApp.Services;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MenuViewModel : ViewModelBase
    {
        private static readonly int _menuPanelOpenWidth = 350;
        private static readonly int _menuPanelCloseWidth = 60;
        private static readonly Bitmap? _openImage = ImageHelper.LoadFromResource(new Uri("avares://AndesiteAvaloniaApp/Assets/Icons/andesite-start.png"));
        private static readonly Bitmap _closeImage = ImageHelper.LoadFromResource(new Uri("avares://AndesiteAvaloniaApp/Assets/Icons/andesite-finish.png"));
        public event Action? OnLoadEditor;
        public event Action? OnCloseEditor;

        [ObservableProperty]
        private static bool _isOpen = false;

        [ObservableProperty]
        private static bool _isMainPageOpen = true;

        [ObservableProperty]
        private ViewModelBase _workPage = ServiceLocator.ServiceProvider.GetRequiredService<NoteListViewModel>();

        [ObservableProperty]
        private int _menuPanelWidth = _menuPanelCloseWidth;

        [ObservableProperty]
        private Bitmap? _menuImage = _closeImage;

        public MenuViewModel()
        {
            OnLoadEditor += MenuViewModel_OnLoadEditor;
            OnCloseEditor += MenuViewModel_OnCloseEditor;

            var noteEditorService = ServiceLocator.ServiceProvider.GetRequiredService<INoteEditorService>();
            noteEditorService.OnLoadEditor = OnLoadEditor;
            noteEditorService.OnCloseEditor = OnCloseEditor;
        }

        private void MenuViewModel_OnCloseEditor()
        {
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<NoteListViewModel>();
        }

        private void MenuViewModel_OnLoadEditor()
        {
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<NoteRedactorViewModel>();
        }

        [RelayCommand]
        private void ChangeMenuState()
        {
            MenuImage = IsOpen ? _closeImage : _openImage;
            MenuPanelWidth = IsOpen ? _menuPanelCloseWidth : _menuPanelOpenWidth;

            IsOpen = !IsOpen;
        }

        [RelayCommand]
        private void OpenWorkPage(string name)
        {
            var viewModelPathString = $"AndesiteAvaloniaApp.ViewModels.{name}ViewModel";
            var viewModelType = Type.GetType(viewModelPathString);

            if (viewModelType is null)
                return;

            var viewModel = ServiceLocator.ServiceProvider.GetRequiredService(viewModelType) as ViewModelBase ?? null!;
            IsMainPageOpen = false;
            WorkPage = viewModel;
        }

        [RelayCommand]
        private void LoadMenuView()
        {
            MenuImage = _closeImage;
            MenuPanelWidth = _menuPanelCloseWidth;
            IsOpen = false;
            IsMainPageOpen = true;
            WorkPage = ServiceLocator.ServiceProvider.GetRequiredService<NoteListViewModel>();
        }
    }
}
