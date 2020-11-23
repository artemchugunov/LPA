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
    public class EditQuestionPageViewModel : BaseNavigationViewModel
    {
        private UserDTO User { get; }
        private int TestId { get; }
        private TestQuestionDTO question;

        public TestQuestionDTO Question { get => question; set { question = value; OnPropertyChanged("Question"); OnPropertyChanged("Text"); OnPropertyChanged("Question.Answers"); } }
        private ICommand saveCmd;
        private ICommand editAnswerCmd;
        private ICommand deleteAnswerCmd;
        private ICommand fetchDataCmd;


        public string Text
        {
            get { return Question.Text; }
            set
            {
                Question.Text = value;
                OnPropertyChanged("Text");
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

        public ICommand EditAnswerCmd
        {
            get
            {
                return editAnswerCmd ?? (editAnswerCmd = new Command(async (object o) => await GetBusyWith(EditAnswer(o))));
            }
            set { }
        }

        public ICommand DeleteAnswerCmd
        {
            get
            {
                return deleteAnswerCmd ?? (deleteAnswerCmd = new Command(async (object o) => await GetBusyWith(DeleteAnswer(o))));
            }
            set { }
        }

        public ICommand FetchDataCmd
        {
            get
            {
                return fetchDataCmd ?? (fetchDataCmd = new Command(async (object o) => await GetBusyWith(FetchData(Question?.Id))));
            }
            set { }
        }

        public EditQuestionPageViewModel(UserDTO user, int? id, int testId)
        {
            User = user;
            TestId = testId;
            Question = new TestQuestionDTO();
            new Task(async () => await GetBusyWith(FetchData(id))).Start();
        }

        public async Task FetchData(int? id)
        {
            if (id == null || id == 0)
            {
                Question = new TestQuestionDTO() { TestId = TestId };
                return;
            }
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"tests/findquestion?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, id));
            if (response != null)
            {
                TestQuestionDTO question = JsonConvert.DeserializeObject<TestQuestionDTO>(response);
                if (question != null)
                {
                    Question = question;
                }
            }
            IsNotBusy = true;
        }

        public async Task Save()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"tests/savequestion?login={0}&password={1}&questionJson={2}".ToUrl(), User.Login, User.Password, Question.ToJson()));
            if (response != null)
            {
                var question = JsonConvert.DeserializeObject<TestQuestionDTO>(response);
                if (question != null)
                {
                    Question = question;
                }
            }
        }

        public async Task EditAnswer(object o)
        {
            var i = o as int?;
            var editAnswerPage = new EditAnswerPage(new EditAnswerPageViewModel(User, i, Question.Id));
            await PushAsync(editAnswerPage);
        }

        public async Task DeleteAnswer(object o)
        {
            var i = o as int?;
            if (i == null)
                return;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(String.Format(@"tests/deleteanswer?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, i.Value));
                if (response.IsSuccessStatusCode)
                {
                    await Save();
                    await FetchData(Question.Id);
                }
            }
        }
    }
}
