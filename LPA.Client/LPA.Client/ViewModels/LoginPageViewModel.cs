using LPA.Client.Views;
using LPA.DTOs;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LPA.Client.ViewModels
{
    public class LoginPageViewModel : BaseNavigationViewModel
    {
        private string login;
        private string password;
        private ICommand loginCmd;
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

        public ICommand LoginCmd
        {
            get
            {
                return loginCmd ?? (loginCmd = new Command(async (object o) => await GetBusyWith(CheckLogin())));
            }
            set { }
        }

        public ICommand RegisterCmd
        {
            get
            {
                return registerCmd ?? (registerCmd = new Command(async (object o) => await GetBusyWith(ShowRegisterPage())));
            }
            set { }
        }
        public LoginPageViewModel()
        {
            IsNotBusy = true;
            Login = "teacher@lpadiploma.com";
            Password = "Teacher";
        }

        public async Task CheckLogin()
        {
            if (String.IsNullOrEmpty(Login) || String.IsNullOrEmpty(Password))
                return;
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"users/finduser?login={0}&password={1}".ToUrl(), Login, Password));
            if (response != null)
            {
                UserDTO user = JsonConvert.DeserializeObject<UserDTO>(response);
                if (user != null)
                {
                    var mainPage = new MainPage(new MainPageViewModel(user));
                    await PushAsync(mainPage);
                }
            }
        }

        public async Task ShowRegisterPage()
        {
            var registerPage = new RegisterPage(new RegisterPageViewModel());
            await PushAsync(registerPage);
        }
    }
}
