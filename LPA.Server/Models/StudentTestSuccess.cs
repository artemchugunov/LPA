using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPA.Server.Models
{
    public class StudentTestSuccess
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
