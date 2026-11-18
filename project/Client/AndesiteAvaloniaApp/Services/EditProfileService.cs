using Avalonia.Controls;
using Avalonia.Xaml.Interactions.Custom;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndesiteAvaloniaApp.Services
{
    public interface IEditProfileService
    {
        Action LogoutAction { get; set; }

        void CloseSession();
    }
    public class EditProfileService() : IEditProfileService
    {
        public Action LogoutAction { get; set; } = null!;

        public void CloseSession()
        {
            LogoutAction?.Invoke();
        }
    }
}
