using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DTOs
{
    public class TestDTO
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public CourseDTO Course { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<TestQuestionDTO> Questions { get; set; }

        //For students
        public bool? IsPassed { get; set; }
    }
}
