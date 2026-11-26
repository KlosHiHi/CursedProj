using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using System;
using System.IO;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class NoteRedactorViewModel(INoteEditorService service) : ViewModelBase
    {
        private readonly INoteEditorService _noteEditorService = service;

        private string editorPath 
            = Path.Combine(AppContext.BaseDirectory, "Controls", "Editor", "index.html");

        [RelayCommand]
        private void OpenEditor(NativeWebView editor)
        {
            editor.Navigate(new Uri(editorPath));
        }
    }
}
