using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LPA.Client.Views;
using LPA.Client.ViewModels;

namespace LPA.Client
{
    public partial class App : Application
    {
#if DEBUG
        public static string UrlPrefix = "http://192.168.1.100/";
#else
        public static string UrlPrefix = "https://lpadiploma.herokuapp.com/";
#endif
        public App()
        {
            InitializeComponent();
            var vm = new LoginPageViewModel();
            MainPage = new NavigationPage( new LoginPage(vm));
            
            //MainPage.DisplayAlert("test",MainPage.BindingContext.ToString(), "OK");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
