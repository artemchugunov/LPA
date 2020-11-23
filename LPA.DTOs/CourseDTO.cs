using System;
using System.Collections.Generic;
using System.Text;

namespace LPA.DTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public UserDTO Author { get; set; }
        public List<TestDTO> Tests { get; set; }
    }
}
