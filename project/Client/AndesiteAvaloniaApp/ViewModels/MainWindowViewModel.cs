using CommunityToolkit.Mvvm.ComponentModel;

namespace AndesiteAvaloniaApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ViewModelBase _currentPage;

        public MainWindowViewModel()
        {
            if (false)
            {
                CurrentPage = new MenuViewModel();
            }
            else
            {
                CurrentPage = new LoginViewModel(); 
            }
        }
    }
}
