using LPA.DTOs;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LPA.Client.ViewModels
{
    public class EditAnswerPageViewModel : BaseNavigationViewModel
    {
        private UserDTO User { get; }
        private TestQuestionAnswerDTO answer;
        private int QuestionId { get; }

        public TestQuestionAnswerDTO Answer
        {
            get { return answer; }
            set
            {
                answer = value;
                OnPropertyChanged("Answer");
                OnPropertyChanged("Text");
                OnPropertyChanged("IsRight");
            }
        }
        private ICommand saveCmd;
        private ICommand fetchDataCmd;


        public string Text
        {
            get { return Answer.Text; }
            set
            {
                Answer.Text = value;
                OnPropertyChanged("Text");
            }
        }

        public bool IsRight
        {
            get { return Answer.IsRight; }
            set
            {
                Answer.IsRight = value;
                OnPropertyChanged("IsRight");
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

        public ICommand FetchDataCmd
        {
            get
            {
                return fetchDataCmd ?? (fetchDataCmd = new Command(async (object o) => await GetBusyWith(FetchData(Answer?.Id))));
            }
            set { }
        }

        public EditAnswerPageViewModel(UserDTO user, int? id, int questionId)
        {
            User = user;
            QuestionId = questionId;
            Answer = new TestQuestionAnswerDTO();
            new Task(async () => await GetBusyWith(FetchData(id))).Start();
        }

        public async Task FetchData(int? id)
        {
            if (id == null || id == 0)
            {
                Answer = new TestQuestionAnswerDTO() { QuestionId = QuestionId };
                return;
            }
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"tests/findanswer?login={0}&password={1}&id={2}".ToUrl(), User.Login, User.Password, id));
            if (response != null)
            {
                TestQuestionAnswerDTO answer = JsonConvert.DeserializeObject<TestQuestionAnswerDTO>(response);
                if (answer != null)
                {
                    Answer = answer;
                }
            }
            IsNotBusy = true;
        }

        public async Task Save()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(String.Format(@"tests/saveanswer?login={0}&password={1}&answerJson={2}".ToUrl(), User.Login, User.Password, Answer.ToJson()));
            if (response != null)
            {
                var answer = JsonConvert.DeserializeObject<TestQuestionAnswerDTO>(response);
                if (answer != null)
                {
                    Answer = answer;
                }
            }
        }
    }
}
