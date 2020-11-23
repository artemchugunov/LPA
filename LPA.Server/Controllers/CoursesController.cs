using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LPA.Server.Models;
using LPA.Server;
using LPA.Server.Helpers;
using LPA.DTOs;
using Newtonsoft.Json;

namespace LPA.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController : ControllerBase
    {

        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ILogger<CoursesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Course> Get()
        {
            return null;
        }

        [HttpGet("GetCourses")]
        public string GetCourses(string login, string password)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var courses = context.Courses.Select(c => new CourseDTO() 
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
            return courses.ToJson();
        }

        [HttpGet("GetMyCourses")]
        public string GetMyCourses(string login, string password)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var courses = (from c in context.Courses
                          join sc in context.StudentCourses on c.Id equals sc.CourseId
                          where sc.StudentId == user.Id
                          select new CourseDTO()
                          {
                              Id = c.Id,
                              Name = c.Name,
                              Description = c.Description
                          }).ToList();
            return courses.ToJson();
        }

        [HttpGet("GetMyAuthoredCourses")]
        public string GetMyAuthoredCourses(string login, string password)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var courses = context.Courses.Where(c => c.AuthorId == user.Id).Select(c => new CourseDTO()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
            return courses.ToJson();
        }

        [HttpGet("FindCourse")]
        public string FindCourse(string login, string password, int id)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var c = context.Courses.FirstOrDefault(c => c.Id == id);
            if (c == null)
                return null;
            var courseDTO = new CourseDTO()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            };
            return courseDTO.ToJson();
        }

        [HttpGet("Enroll")]
        public ActionResult Enroll(string login, string password, int id)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return new UnauthorizedResult();
            var course = context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null)
                return new UnauthorizedResult();
            if (context.StudentCourses.FirstOrDefault(sc => sc.StudentId == user.Id && sc.CourseId == course.Id) == null)
            {
                context.StudentCourses.Add(new StudentCourse() { CourseId = course.Id, StudentId = user.Id });
                context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("SaveCourse")]
        public string SaveCourse(string login, string password, string courseJson)
        {
            var course = JsonConvert.DeserializeObject<CourseDTO>(courseJson);
            if (course == null)
                return null;
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null || !(user.IsTeacher || user.IsAdmin))
                return null;
            var oldCourse = context.Courses.FirstOrDefault(c => c.Id == course.Id);
            if (oldCourse == null)
            {
                oldCourse = new Course() { AuthorId = user.Id };
                context.Courses.Add(oldCourse);
            }
                if (oldCourse.AuthorId == user.Id)
                {
                    oldCourse.Name = course.Name;
                    oldCourse.Description = course.Description;
                }
                else return null;
            
            context.SaveChanges();
            course.Description = oldCourse.Description;
            course.Id = oldCourse.Id;
            course.Name = oldCourse.Name;
            return course.ToJson();
        }

        [HttpGet("DeleteCourse")]
        public ActionResult DeleteCourse(string login, string password, int id)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null || !(user.IsTeacher || user.IsAdmin))
                return Unauthorized();
            var oldCourse = context.Courses.FirstOrDefault(c => c.Id == id);
            if (oldCourse == null)
                return BadRequest();
            if (oldCourse.AuthorId != user.Id && !user.IsAdmin)
                return Unauthorized();
            context.Courses.Remove(oldCourse);
            context.SaveChanges();
            return Ok();
        }
    }
}
