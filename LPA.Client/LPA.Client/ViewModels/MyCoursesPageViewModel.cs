using LPA.Client.Views;
using LPA.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LPA.Client.ViewModels
{
    public class MyCoursesPageViewModel : BaseNavigationViewModel
    {
        private UserDTO User { get; }
        private ObservableCollection<CourseDTO> courses;
        private ICommand fetchDataCmd;
        private ICommand getTestsCmd;
        public ObservableCollection<CourseDTO> Courses
        {
            get { return courses; }
            set
            {
                courses = value;
                OnPropertyChanged("Courses");
            }
        }

        public ICommand FetchDataCmd
        {
            get
            {
                return fetchDataCmd ?? (fetchDataCmd = new Command(async (object o) => await GetBusyWith(FetchData())));
            }
            set { }
        }

        public ICommand GetTestsCmd
        {
            get
            {
                return getTestsCmd ?? (getTestsCmd = new Command(async (object o) => await GetBusyWith(GetTests(o))));
            }
            set { }
        }

        public MyCoursesPageViewModel(UserDTO user)
        {
            User = user;
            new Task(async () => await FetchData()).Start();
        }

        public async Task FetchData()
        {
            using (var client = new HttpClient())
            {
                string temp = await client.GetStringAsync(String.Format(@"courses/getmycourses?login={0}&password={1}".ToUrl(), User.Login, User.Password));
                Courses = new ObservableCollection<CourseDTO>(JsonConvert.DeserializeObject<List<CourseDTO>>(temp));
            }
            IsNotBusy = true;
        }

        public async Task GetTests(object o)
        {
            int? id = o as int?;
            if (id == null || id <= 0)
                return;
            TestsForStudentPage testsPage = new TestsForStudentPage(new TestsForStudentPageViewModel(User, id.Value));
            await PushAsync(testsPage);
        }
    }
}
