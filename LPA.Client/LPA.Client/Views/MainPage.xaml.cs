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
    public partial class MainPage : TabbedPage
    {
        public MainPageViewModel ViewModel { get; }
        public MainPage(MainPageViewModel vm)
        {
            BindingContext = this;
            ViewModel = vm;
            Children.Add(new CoursesPage(new CoursesPageViewModel(ViewModel.User)));
            Children.Add(new MyCoursesPage(new MyCoursesPageViewModel(ViewModel.User)));
            if (ViewModel.User.IsTeacher)
            {
                Children.Add(new MyAuthoredCoursesPage(new MyAuthoredCoursesPageViewModel(ViewModel.User)));
            }
            if (ViewModel.User.IsAdmin)
            {

            }
            InitializeComponent();
            BindingContext = this;
        }
    }
}