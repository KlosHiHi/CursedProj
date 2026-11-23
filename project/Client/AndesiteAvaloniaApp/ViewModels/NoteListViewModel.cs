using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace AndesiteAvaloniaApp.ViewModels
{
    public class PresentNote
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public bool IsMerge { get; set; }
    }

    public partial class NoteListViewModel : ViewModelBase
    {
        [ObservableProperty]
        private List<PresentNote> _notes =
        [
            new() { Title = "Список покупок", Content = "Молоко, хлеб, яйца, сыр, помидоры", IsMerge = false },
            new() { Title = "Встреча с командой", Content = "Обсудить спринт, задачи на неделю и блокеры", IsMerge = true  },
            new() { Title = "Идеи для проекта", Content = "Тёмная тема, экспорт в PDF, офлайн-режим", IsMerge = false },
            new() { Title = "Книги к прочтению", Content = "Чистый код, Рефакторинг, Паттерны проектирования", IsMerge = false },
            new() { Title = "План тренировки", Content = "Разминка 10 мин, кардио 20 мин, силовые 15 мин", IsMerge = true  },
            new() { Title = "Список покупок", Content = "Молоко, хлеб, яйца, сыр, помидоры", IsMerge = false },
            new() { Title = "Встреча с командой", Content = "Обсудить спринт, задачи на неделю и блокеры", IsMerge = true  },
            new() { Title = "Идеи для проекта", Content = "Тёмная тема, экспорт в PDF, офлайн-режим", IsMerge = false },
            new() { Title = "Книги к прочтению", Content = "Чистый код, Рефакторинг, Паттерны проектирования", IsMerge = false },
            new() { Title = "План тренировки", Content = "Разминка 10 мин, кардио 20 мин, силовые 15 мин", IsMerge = true  },
            new() { Title = "Список покупок", Content = "Молоко, хлеб, яйца, сыр, помидоры", IsMerge = false },
            new() { Title = "Встреча с командой", Content = "Обсудить спринт, задачи на неделю и блокеры", IsMerge = true  },
            new() { Title = "Идеи для проекта", Content = "Тёмная тема, экспорт в PDF, офлайн-режим", IsMerge = false },
            new() { Title = "Книги к прочтению", Content = "Чистый код, Рефакторинг, Паттерны проектирования", IsMerge = false },
            new() { Title = "План тренировки", Content = "Разминка 10 мин, кардио 20 мин, силовые 15 мин", IsMerge = true  },
        ];
    }
}
