using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? WhenDeleted { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsTeacher { get; set; }
        public DateTime? LastActionTime { get; set; }

        public List<CourseDTO> CoursesAuthored { get; set; }
    }
}
