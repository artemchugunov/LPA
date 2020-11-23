using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LPA.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Login { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? WhenDeleted { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsTeacher { get; set; }
        public DateTime? LastActionTime { get; set; }

        public ICollection<StudentCourse> Courses { get; set; }
        public ICollection<Course> CoursesAuthored { get; set; }

        public User()
        {
            Courses = new List<StudentCourse>();
            CoursesAuthored = new List<Course>();
        }
    }
}
