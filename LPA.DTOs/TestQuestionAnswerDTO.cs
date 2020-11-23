using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DTOs
{
    public class TestQuestionAnswerDTO
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public TestQuestionDTO Question { get; set; }
        public string Text { get; set; }
        public bool IsRight { get; set; }
        public bool IsChecked { get; set; }
    }
}
