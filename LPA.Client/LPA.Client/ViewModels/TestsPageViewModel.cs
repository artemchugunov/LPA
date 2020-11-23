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
    public class TestsPageViewModel : BaseNavigationViewModel
    {
        private int CourseId { get; }
        private UserDTO User { get; }
        private ObservableCollection<TestDTO> tests;
        private ICommand fetchDataCmd;
        private ICommand editTestCmd;
        private ICommand deleteTestCmd;

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

        public ICommand EditTestCmd
        {
            get
            {
                return editTestCmd ?? (editTestCmd = new Command(async (object o) => await GetBusyWith(EditTest(o))));
            }
            set { }
        }

        public ICommand DeleteTestCmd
        {
            get
            {
                return deleteTestCmd ?? (deleteTestCmd = new Command(async (object o) => await GetBusyWith(DeleteTest(o))));
            }
            set { }
        }

        public TestsPageViewModel(UserDTO user, int courseId)
        {
            User = user;
            CourseId = courseId;
            new Task(async () => await FetchData()).Start();
        }

        public async Task FetchData()
        {
            using (var client = new HttpClient())
            {
                string temp = await client.GetStringAsync(String.Format(@"tests/getauthoredtestsforcourse?login={0}&password={1}&courseid={2}".ToUrl(), User.Login, User.Password, CourseId));
                Tests = new ObservableCollection<TestDTO>(temp.FromJson<List<TestDTO>>());
            }
            IsNotBusy = true;
        }

        public async Task EditTest(object o)
        {
            var id = o as int?;
            EditTestPage editTestPage = new EditTestPage(new EditTestPageViewModel(User, id, CourseId));
            await PushAsync(editTestPage);
        }

        public async Task DeleteTest(object o)
        {
            var i = o as int?;
            if (i == null)
                return;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(String.Format(@"tests/deletetest?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, i.Value));
                if (response.IsSuccessStatusCode)
                {
                    await FetchData();
                }
            }
        }
    }
}
