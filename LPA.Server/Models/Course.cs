using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LPA.Server.Models
{
    public class Course
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(140)]
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public ICollection<StudentCourse> Students { get; set; }

        public Course()
        {
            Students = new List<StudentCourse>();
        }
    }
}
