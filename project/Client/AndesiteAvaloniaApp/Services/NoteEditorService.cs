using AndesiteAvaloniaApp.ViewModels;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using OxideClassLibrary.DTOs;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.Services
{
    public interface INoteEditorService
    {
        Action OnLoadEditor { get; set; }
        Action OnCloseEditor { get; set; }
        NoteDto? Note { get; set; }

        void OpenEditorPage(NoteDto note);
        Task CloseEditorPage();
    }

    public class NoteEditorService : INoteEditorService
    {
        public Action OnLoadEditor { get; set; } = null!;
        public Action OnCloseEditor { get; set; } = null!;
        public NoteDto? Note { get; set; }

        public void OpenEditorPage(NoteDto note)
        {
            Note = note;
            OnLoadEditor?.Invoke();
        }        

        public async Task CloseEditorPage()
        {
            OnCloseEditor?.Invoke();
        }
    }
}
