using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DTOs
{
    public class TestQuestionDTO
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public TestDTO Test { get; set; }
        public string Text { get; set; }
        public bool IsOpen { get; set; }
        public bool AreMultipleAnswersRight { get; set; }
        public List<TestQuestionAnswerDTO> Answers { get; set; }
    }
}
