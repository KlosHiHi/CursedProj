using AndesiteAvaloniaApp.Services;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OxideClassLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AndesiteAvaloniaApp.ViewModels
{
    public class Rootobject
    {
        public string type { get; set; }
        public Content[] content { get; set; }
    }

    public class Content
    {
        public string type { get; set; }
        public Content1[] content { get; set; }
    }

    public class Content1
    {
        public string type { get; set; }
        public string text { get; set; }
    }

    public class PresentNote
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool IsMerge { get; set; }
    }

    public partial class NoteListViewModel(INoteEditorService service) : ViewModelBase
    {
        private readonly INoteEditorService _noteEditorService = service;


        [ObservableProperty]
        private List<PresentNote> _notes =
        [
            new() { Id = new Guid(), Title = "Список покупок", Content = "Молоко, хлеб, яйца, сыр, помидоры", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(), Title = "Встреча с командой", Content = "Обсудить спринт, задачи на неделю и блокеры", IsMerge = true, CategoryId = new Guid()  },
            new() { Id = new Guid(), Title = "Идеи для проекта", Content = "Тёмная тема, экспорт в PDF, офлайн-режим", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(), Title = "Книги к прочтению", Content = "Чистый код, Рефакторинг, Паттерны проектирования", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(),  Title = "План тренировки", Content = "Разминка 10 мин, кардио 20 мин, силовые 15 мин", IsMerge = false, CategoryId = new Guid()  },
            new() { Id = new Guid(),  Title = "Список покупок", Content = "Молоко, хлеб, яйца, сыр, помидоры", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(),  Title = "Встреча с командой", Content = "Обсудить спринт, задачи на неделю и блокеры", IsMerge = true, CategoryId = new Guid()  },
            new() { Id = new Guid(),  Title = "Идеи для проекта", Content = "Тёмная тема, экспорт в PDF, офлайн-режим", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(),  Title = "Книги к прочтению", Content = "Чистый код, Рефакторинг, Паттерны проектирования", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(),  Title = "План тренировки", Content = "Разминка 10 мин, кардио 20 мин, силовые 15 мин", IsMerge = false, CategoryId = new Guid()  },
            new() { Id = new Guid(),  Title = "Список покупок", Content = "Молоко, хлеб, яйца, сыр, помидоры", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(),  Title = "Встреча с командой", Content = "Обсудить спринт, задачи на неделю и блокеры", IsMerge = false, CategoryId = new Guid()  },
            new() { Id = new Guid(),  Title = "Идеи для проекта", Content = "Тёмная тема, экспорт в PDF, офлайн-режим", IsMerge = false },
            new() { Id = new Guid(),  Title = "Книги к прочтению", Content = "Чистый код, Рефакторинг, Паттерны проектирования", IsMerge = false, CategoryId = new Guid() },
            new() { Id = new Guid(),  Title = "План тренировки", Content = "Разминка 10 мин, кардио 20 мин, силовые 15 мин", IsMerge = false, CategoryId = new Guid()  },
        ];

        [RelayCommand]
        private void OpenNote(object? args)
        {
            if (args is TappedEventArgs e)
            {
                if (e.Source is Border border)
                {
                    var selectNote = border.DataContext as PresentNote;

                    var json = new Rootobject()
                    {
                        type = "doc",
                        content = [
                            new() {
                                type = "paragraph",
                                content = [
                                    new() {
                                        type = "text",
                                        text = "тестовый"
                                    }]
                            }]
                    };


                    var note = new NoteDto()
                    {
                        Title = selectNote.Title,
                        Content = JsonSerializer.Serialize(json)
                    };

                    _noteEditorService.OpenEditorPage(note);
                }
            }
        }
    }
}
