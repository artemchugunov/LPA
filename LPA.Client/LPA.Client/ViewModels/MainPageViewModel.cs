//using MvvmHelpers;
//using MvvmHelpers.Commands;
using LPA.DTOs;

namespace LPA.Client.ViewModels
{
    public class MainPageViewModel : BaseNavigationViewModel
    {
        public UserDTO User { get; }

        public MainPageViewModel(UserDTO user)
        {
            User = user;
        }
    }
}
