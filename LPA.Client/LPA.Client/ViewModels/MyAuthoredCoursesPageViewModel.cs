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
    public class MyAuthoredCoursesPageViewModel : BaseNavigationViewModel
    {
        private UserDTO User { get; }
        private ObservableCollection<CourseDTO> courses;
        private CourseDTO selectedCourse;
        private ICommand editCmd;
        private ICommand fetchDataCmd;
        private ICommand deleteCmd;
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

        public ICommand EditCmd
        {
            get
            {
                return editCmd ?? (editCmd = new Command(async (object o) => await GetBusyWith(Edit(o))));
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

        public ICommand DeleteCmd
        {
            get
            {
                return deleteCmd ?? (deleteCmd = new Command(async (object o) => await GetBusyWith(Delete(o))));
            }
            set { }
        }

        public MyAuthoredCoursesPageViewModel(UserDTO user)
        {
            User = user;
            new Task(async () => await GetBusyWith(FetchData())).Start();
        }

        public async Task FetchData()
        {
            using (var client = new HttpClient())
            {
                string temp = await client.GetStringAsync(String.Format(@"courses/getmyauthoredcourses?login={0}&password={1}".ToUrl(), User.Login, User.Password));
                var coll = JsonConvert.DeserializeObject<List<CourseDTO>>(temp, new JsonSerializerSettings() { Formatting = Formatting.Indented });
                Courses = new ObservableCollection<CourseDTO>(coll);
            }
        }

        public async Task Edit(object o)
        {
            int? id = o as int?;
            var editCoursePage = new EditCoursePage(new EditCoursePageViewModel(User, id));
            await PushAsync(editCoursePage);
        }

        public async Task Delete(object o)
        {
            using (var client = new HttpClient())
            {
                int? id = o as int?;
                var result = await client.GetAsync(String.Format(@"courses/deletecourse?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, id));
                if (result.IsSuccessStatusCode)
                    await FetchData();
            }
            IsNotBusy = true;
        }
    }
}
