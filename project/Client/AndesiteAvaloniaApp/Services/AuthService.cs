using Avalonia.Controls;
using System;

namespace AndesiteAvaloniaApp.Services
{
    public class AuthService() : IAuthService
    {
        public Window MainWindow { get; set; } = null!;
        public Action<string> LoginAction { get; set; } = null!;

        public void CompleteWork(Window callWindow, string workType)
        {
            LoginAction?.Invoke(workType);
            callWindow!.Close();
        }
    }
}
