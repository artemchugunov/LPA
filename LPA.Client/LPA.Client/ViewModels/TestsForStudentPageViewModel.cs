using LPA.Client.Views;
using LPA.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LPA.Client.ViewModels
{
    public class TestsForStudentPageViewModel : BaseNavigationViewModel
    {
        private int CourseId { get; }
        private UserDTO User { get; }
        private ObservableCollection<TestDTO> tests;
        private ICommand fetchDataCmd;
        private ICommand passTestCmd;

        public ObservableCollection<TestDTO> Tests
        {
            get { return tests; }
            set
            {
                tests = value;
                OnPropertyChanged("Tests");
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

        public ICommand PassTestCmd
        {
            get
            {
                return passTestCmd ?? (passTestCmd = new Command(async (object o) => await GetBusyWith(PassTest(o))));
            }
            set { }
        }

        public TestsForStudentPageViewModel(UserDTO user, int courseId)
        {
            User = user;
            CourseId = courseId;
            new Task(async () => await GetBusyWith(FetchData())).Start();
        }

        public async Task FetchData()
        {
            using (var client = new HttpClient())
            {
                string temp = await client.GetStringAsync(String.Format(@"tests/gettestsforcourse?login={0}&password={1}&courseid={2}".ToUrl(), User.Login, User.Password, CourseId));
                Tests = new ObservableCollection<TestDTO>(temp.FromJson<List<TestDTO>>());
            }
        }

        public async Task PassTest(object o)
        {
            var id = o as int?;
            if (id == null || id == 0)
                return;
            PassTestPage passTestPage = new PassTestPage(new PassTestPageViewModel(User, id.Value));
            await PushAsync(passTestPage);
        }
    }
}
