using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class NoteViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isMerge = true;

        [ObservableProperty]
        private string _title = "Заголовок";

        [ObservableProperty]
        private string _content = "Содержание";

    }
}
