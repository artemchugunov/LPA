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
    public class EditTestPageViewModel : BaseNavigationViewModel
    {
        private int CourseId { get; }
        private UserDTO User { get; }

        public TestDTO Test { get => test; set { test = value; OnPropertyChanged("Test"); OnPropertyChanged("Name"); OnPropertyChanged("Description"); OnPropertyChanged("Test.Questions"); } }
        private ICommand saveCmd;
        private ICommand editQuestionCmd;
        private ICommand deleteQuestionCmd;
        private ICommand fetchDataCmd;
        private TestDTO test;

        public string Name
        {
            get { return Test.Name; }
            set
            {
                Test.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description
        {
            get { return Test.Description; }
            set
            {
                Test.Description = value;
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

        public ICommand EditQuestionCmd
        {
            get
            {
                return editQuestionCmd ?? (editQuestionCmd = new Command(async (object o) => await GetBusyWith(EditQuestion(o))));
            }
            set { }
        }

        public ICommand DeleteQuestionCmd
        {
            get
            {
                return deleteQuestionCmd ?? (deleteQuestionCmd = new Command(async (object o) => await GetBusyWith(DeleteQuestion(o))));
            }
            set { }
        }

        public ICommand FetchDataCmd
        {
            get
            {
                return fetchDataCmd ?? (fetchDataCmd = new Command(async (object o) => await GetBusyWith(FetchData(Test?.Id))));
            }
            set { }
        }

        public EditTestPageViewModel(UserDTO user, int? id, int courseId)
        {
            CourseId = courseId;
            User = user;
            Test = new TestDTO();
            new Task(async () => await GetBusyWith(FetchData(id))).Start();

        }

        public async Task FetchData(int? id)
        {
            if (id == null || id == 0)
            {
                Test = new TestDTO() { CourseId = CourseId };
                return;
            }
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"tests/findtest?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, id));
            if (response != null)
            {
                TestDTO test = JsonConvert.DeserializeObject<TestDTO>(response);
                if (test != null)
                {
                    Test = test;
                }
            }
            IsNotBusy = true;
        }

        public async Task Save()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"tests/savetest?login={0}&password={1}&testJson={2}".ToUrl(), User.Login, User.Password, Test.ToJson()));
            if (response != null)
            {
                var test = JsonConvert.DeserializeObject<TestDTO>(response);
                if (test != null)
                {
                    Test = test;
                }
            }
        }

        public async Task EditQuestion(object o)
        {
            await Save();
            var i = o as int?;
            var editQuestionPage = new EditQuestionPage(new EditQuestionPageViewModel(User, i, Test.Id));
            await PushAsync(editQuestionPage);
        }

        public async Task DeleteQuestion(object o)
        {
            var i = o as int?;
            if (i == null)
                return;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(String.Format(@"tests/deletequestion?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, i.Value));
                if (response.IsSuccessStatusCode)
                {
                    await Save();
                    await FetchData(Test.Id);
                }
            }
        }
    }
}
