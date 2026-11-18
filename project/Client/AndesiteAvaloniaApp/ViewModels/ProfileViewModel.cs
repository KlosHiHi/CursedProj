using AndesiteAvaloniaApp.Services;
using CommunityToolkit.Mvvm.Input;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class ProfileViewModel(IEditProfileService service) : ViewModelBase
    {
        private readonly IEditProfileService _editService = service;


        [RelayCommand]
        private void LogOut()
        {
            TokenStorage.Instance.Clear();
            _editService.CloseSession();
        }
    }
}
