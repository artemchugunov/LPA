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
    public class CoursesPageViewModel : BaseNavigationViewModel
    {
        private UserDTO User { get; }
        private ObservableCollection<CourseDTO> courses;
        private CourseDTO selectedCourse;
        private ICommand enrollCmd;
        private ICommand fetchDataCmd;
        public ObservableCollection<CourseDTO> Courses
        {
            get { return courses; }
            set
            {
                courses = value;
                OnPropertyChanged("Courses");
            }
        }

        public CourseDTO SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                selectedCourse = value;
                OnPropertyChanged("SelectedCourse");
            }
        }

        public ICommand EnrollCmd
        {
            get
            {
                return enrollCmd ?? (enrollCmd = new Command(async (object o) => await GetBusyWith(Enroll(o))));
            }
            set { }
        }

        public ICommand FetchDataCmd
        {
            get
            {
                return fetchDataCmd ?? (fetchDataCmd = new Command(async (object o) => await GetBusyWith(FetchData())));
            }
            set { }
        }

        public CoursesPageViewModel(UserDTO user)
        {
            User = user;
            new Task(async () => await FetchData()).Start();
        }

        public async Task FetchData()
        {
            using (var client = new HttpClient())
            {
                string temp = await client.GetStringAsync(String.Format(@"courses/getcourses?login={0}&password={1}".ToUrl(), User.Login, User.Password));
                var coll = JsonConvert.DeserializeObject<List<CourseDTO>>(temp, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                Courses = new ObservableCollection<CourseDTO>(coll);
            }
            IsNotBusy = true;

        }

        public async Task Enroll(object o)
        {
            int? id = o as int?;
            if (id != null)
            {
                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(String.Format(@"courses/enroll?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, id.Value));
                }
            }
        }
    }
}
