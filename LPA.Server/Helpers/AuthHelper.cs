using System.Linq;
using LPA.Server.Models;

namespace LPA.Server.Helpers
{
    public class AuthHelper
    {
        public static User FindUser(string login, string password, MyContext context)
        {
            return context?.Users.FirstOrDefault(u => u.Login == login && u.Password == password && u.IsDeleted == false);
        }
    }
}
