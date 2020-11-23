using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPA.Server.Models
{
    public class TestQuestionAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public TestQuestion Question { get; set; }
        public string Text { get; set; }
        public bool IsRight { get; set; }
    }
}
