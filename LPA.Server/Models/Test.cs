using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LPA.Server.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(140)]
        public string Description { get; set; }
        public ICollection<TestQuestion> Questions { get; set; }
        public ICollection<StudentTestSuccess> TestSuccesses { get; set; }

        public Test()
        {
            Questions = new List<TestQuestion>();
            TestSuccesses = new List<StudentTestSuccess>();
        }
    }
}
