using System;

namespace AndesiteAvaloniaApp.Services
{
    public interface IEditProfileService
    {
        Action LogoutAction { get; set; }

        void CloseSession();
    }
}