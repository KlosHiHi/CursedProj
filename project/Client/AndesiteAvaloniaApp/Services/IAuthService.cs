using Avalonia.Controls;
using System;

namespace AndesiteAvaloniaApp.Services
{
    public interface IAuthService
    {
        Action<string> LoginAction { get; set; }
        Window MainWindow { get; set; }

        void CompleteWork(Window callWindow, string workType);
    }
}