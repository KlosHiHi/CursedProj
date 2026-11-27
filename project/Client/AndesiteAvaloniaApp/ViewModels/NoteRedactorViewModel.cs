using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OxideClassLibrary.DTOs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class NoteRedactorViewModel(INoteEditorService service) : ViewModelBase
    {
        private readonly INoteEditorService _noteEditorService = service;

        [ObservableProperty]
        public NoteDto _currentNote = new();

        private string editorPath = Path.Combine(Environment.CurrentDirectory, "Assets", "Editor", "editor.html");

        [RelayCommand]
        private async Task OpenEditorWeb(NativeWebView webView)
        {
            if (webView == null)
                return;

            webView.Navigate(new Uri(editorPath));

            CurrentNote = _noteEditorService.Note!;

            webView.NavigationCompleted += WebView_NavigationCompleted;
        }

        private async void WebView_NavigationCompleted(object? sender, WebViewNavigationCompletedEventArgs e)
        {
            var webView = sender as NativeWebView;
            var result = await webView!.InvokeScript($"window.noteEditor.setContent({CurrentNote.Content});");
        }

        private async Task<string?> GetNoteContentAsync(NativeWebView webView)
        {
            if (webView == null)
                return null;

            var content = await webView.InvokeScript($"JSON.stringify(window.noteEditor.getJSON());");
            return content;
        }

        [RelayCommand]
        private async Task CloseEditorWebAsync(NativeWebView webView)
        {
            await _noteEditorService.CloseEditorPage();

            var content = await GetNoteContentAsync(webView);

            if (content != null)
                CurrentNote.Content = content;
        }
    }
}
