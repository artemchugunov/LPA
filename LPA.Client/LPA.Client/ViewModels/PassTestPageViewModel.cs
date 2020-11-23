using LPA.DTOs;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LPA.Client.ViewModels
{
    public class PassTestPageViewModel : BaseNavigationViewModel
    {
        private TestQuestionDTO currentQuestion;
        private int index;
        private ICommand nextQuestionCmd;
        private ICommand previousQuestionCmd;
        private ICommand checkTestCmd;
        private UserDTO User { get; }
        public TestDTO Test { get; private set; }
        public TestQuestionDTO CurrentQuestion
        {
            get => currentQuestion;
            set
            {
                currentQuestion = value;
                OnPropertyChanged("CurrentQuestion");
            }
        }

        public ICommand CheckTestCmd
        {
            get
            {
                return checkTestCmd ?? (checkTestCmd = new Command(async (object o) => await GetBusyWith(CheckTest())));
            }
            set { }
        }

        public ICommand NextQuestionCmd
        {
            get
            {
                return nextQuestionCmd ?? (nextQuestionCmd = new Command((object o) => NextQuestion()));
            }
            set { }
        }

        public ICommand PreviousQuestionCmd
        {
            get
            {
                return previousQuestionCmd ?? (previousQuestionCmd = new Command((object o) => PreviousQuestion()));
            }
            set { }
        }

        public PassTestPageViewModel(UserDTO user, int id)
        {
            User = user;
            Test = new TestDTO();
            new Task(async () => await GetBusyWith(FetchData(id))).Start();
        }

        public async Task FetchData(int id)
        {
            using (var client = new HttpClient())
            {
                string temp = await client.GetStringAsync(String.Format(@"tests/findtest?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, id));
                Test = temp.FromJson<TestDTO>();
                if (Test?.Questions != null && Test.Questions.Count > 0)
                {
                    CurrentQuestion = Test.Questions[0];
                    index = 0;
                }
            }
        }

        public async Task CheckTest()
        {
            using (var client = new HttpClient())
            {
                string temp = await client.GetStringAsync(String.Format(@"tests/checktest?login={0}&password={1}&testJson={2}".ToUrl(), User.Login, User.Password, Test.ToJson()));
                Test = temp.FromJson<TestDTO>();
            }
        }

        public void NextQuestion()
        {
            if (index < Test.Questions.Count - 1)
            {
                index++;
                CurrentQuestion = Test.Questions[index];
            }
        }

        public void PreviousQuestion()
        {
            if (index > 0)
            {
                index--;
                CurrentQuestion = Test.Questions[index];
            }
        }
    }
}
