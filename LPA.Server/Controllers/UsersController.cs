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

namespace LPA.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<UserDTO> Get()
        {
            return null;
        }

        [HttpGet("GetUsers")]
        public string GetUsers(string login, string password)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var users = context.Users.Where(u => !u.IsDeleted).Select(u => new User() { Login = u.Login }).ToList();
            return users.ToJson();
        }

        [HttpGet("FindUser")]
        public string FindUser(string login, string password)
        {
            using MyContext context = new MyContext();
            var user = AuthHelper.FindUser(login, password, context);
            if (user == null)
                return null;
            var userDTO = new UserDTO()
            {
                Login = user.Login,
                Password = user.Password,
                IsAdmin = user.IsAdmin,
                IsTeacher = user.IsTeacher,
                LastActionTime = user.LastActionTime
            };
            return userDTO.ToJson();
        }

        [HttpGet("Register")]
        public string Register(string login, string password)
        {
            using MyContext context = new MyContext();
            var user = context.Users.FirstOrDefault(u => u.Login == login);
            if (user != null)
                return null;
            user = new User()
            {
                Login = login,
                Password = password,
                LastActionTime = DateTime.Now
            };
            context.Users.Add(user);
            context.SaveChanges();
            var userDTO = new UserDTO()
            {
                Login = user.Login,
                Password = user.Password,
                LastActionTime = user.LastActionTime
            };
            return userDTO.ToJson();
        }

        [HttpGet("SaveUser")]
        public string SaveUser(string login, string password, string userJson)
        {
            var userDTO = userJson.FromJson<UserDTO>();
            if (userDTO == null)
            {
                return null;
            }
            using MyContext context = new MyContext();
            var currentUser = AuthHelper.FindUser(login, password, context);
            if (currentUser?.IsAdmin != true)
                return null;
            var user = context.Users.FirstOrDefault(u => u.Id == userDTO.Id);
            if (user == null)
            {
                user = new User();
                context.Users.Add(user);
            }
            user.Login = userDTO.Login;
            user.Password = userDTO.Password;
            user.IsTeacher = userDTO.IsTeacher;
            if (user.IsDeleted != userDTO.IsDeleted)
            {
                if (userDTO.IsDeleted)
                    user.WhenDeleted = DateTime.Now;
                else
                    user.WhenDeleted = null;
                user.IsDeleted = userDTO.IsDeleted;
            }
            context.SaveChanges();
            userDTO.Id = user.Id;
            return userDTO.ToJson();
        }
    }
}
