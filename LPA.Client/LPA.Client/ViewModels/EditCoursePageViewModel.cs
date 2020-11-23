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
    public class EditCoursePageViewModel : BaseNavigationViewModel
    {
        private UserDTO User { get; }

        public CourseDTO Course { get => course; set { course = value; OnPropertyChanged("Course"); OnPropertyChanged("Name"); OnPropertyChanged("Description"); } }
        private ICommand saveCmd;
        private ICommand getTestsCmd;
        private ICommand fetchDataCmd;
        private CourseDTO course;

        public string Name
        {
            get { return Course.Name; }
            set
            {
                Course.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return Course.Description; }
            set
            {
                Course.Description = value;
                OnPropertyChanged("Description");
            }
        }

        public ICommand SaveCmd
        {
            get
            {
                return saveCmd ?? (saveCmd = new Command(async (object o) => await GetBusyWith(Save())));
            }
            set { }
        }

        public ICommand GetTestsCmd
        {
            get
            {
                return getTestsCmd ?? (getTestsCmd = new Command(async (object o) => await GetBusyWith(GetTests())));
            }
            set { }
        }

        public ICommand FetchDataCmd
        {
            get
            {
                return fetchDataCmd ?? (fetchDataCmd = new Command(async (object o) => await GetBusyWith(FetchData(Course?.Id))));
            }
            set { }
        }

        public EditCoursePageViewModel(UserDTO user, int? id)
        {
            User = user;
            Course = new CourseDTO();
            new Task(async () => await GetBusyWith(FetchData(id))).Start();
        }

        public async Task FetchData(int? id)
        {
            if (id == null || id == 0)
            {
                Course = new CourseDTO();
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                string address = String.Format(@"courses/findcourse?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, id);
                var response = await client.GetStringAsync(address);
                if (response != null)
                {
                    CourseDTO course = JsonConvert.DeserializeObject<CourseDTO>(response);
                    if (course != null)
                    {
                        Course = course;
                    }
                }
            }
        }

        public async Task Save()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(String.Format(@"courses/savecourse?login={0}&password={1}&courseJson={2}".ToUrl(), User.Login, User.Password, Course.ToJson()));
                if (response != null)
                {
                    var course = JsonConvert.DeserializeObject<CourseDTO>(response);
                    if (course != null)
                    {
                        Course = course;
                    }
                }
            }
        }

        public async Task GetTests()
        {
            if (Course == null)
                return;
            await Save();
            if (Course.Id > 0)
            {
                TestsPage testsPage = new TestsPage(new TestsPageViewModel(User, Course.Id));
                await PushAsync(testsPage);
            }
        }
    }
}
