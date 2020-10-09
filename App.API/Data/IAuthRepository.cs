using System.Threading.Tasks;
using App.API.Models;

namespace App.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string passWord);
          Task<User> LogIn(string username , string passWord);
         Task<bool> UserExists(string username);

         
    }
}