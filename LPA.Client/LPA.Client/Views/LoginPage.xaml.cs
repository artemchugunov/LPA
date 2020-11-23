using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPA.Client.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LPA.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPageViewModel ViewModel { get; }
        public LoginPage(LoginPageViewModel vm)
        {
            BindingContext = this;
            ViewModel = vm;
            InitializeComponent();
            BindingContext = this;
            ViewModel = vm;
        }
    }
}