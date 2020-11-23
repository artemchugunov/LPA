using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
//using MvvmHelpers;

namespace LPA.Client.ViewModels
{
    public class RegisterPageViewModel : BaseNavigationViewModel
    {
        private string login;
        private string password;
        private ICommand registerCmd;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public ICommand RegisterCmd
        {
            get
            {
                return registerCmd ?? (registerCmd = new Command(async (object o) => await GetBusyWith(Register())));
            }
            set { }
        }

        public RegisterPageViewModel()
        {
            IsNotBusy = true;
        }

        private async Task Register()
        {
            if (String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Password))
                return;
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"users/register?login={0}&password={1}".ToUrl(), Login, Password));
            if (response != null)
            {
                await PopAsync();
            }
        }
    }
}
