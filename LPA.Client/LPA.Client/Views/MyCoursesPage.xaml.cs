using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LPA.Client.ViewModels;

namespace LPA.Client.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCoursesPage : ContentPage
    {
        public MyCoursesPageViewModel ViewModel { get; }
        public MyCoursesPage(MyCoursesPageViewModel vm)
        {
            BindingContext = this;
            ViewModel = vm;
            InitializeComponent();
            BindingContext = this;
            ViewModel = vm;
        }
    }
}