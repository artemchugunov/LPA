using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPA.Server.Models
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public string Text { get; set; }
        public bool IsOpen { get; set; }
        public bool AreMultipleAnswersRight { get; set; }
        public ICollection<TestQuestionAnswer> Answers { get; set; }

        public TestQuestion()
        {
            Answers = new List<TestQuestionAnswer>();
        }
    }
}
