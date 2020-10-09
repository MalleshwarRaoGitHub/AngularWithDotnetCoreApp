using System;
using System.Threading.Tasks;
using App.API.Models;
using Microsoft.EntityFrameworkCore;

namespace App.API.Data
{
    public class AuthRepository: IAuthRepository
    {

private readonly DataContext _context;
public AuthRepository(DataContext context)
{
    _context = context;
}
      
        public async  Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordsalt;
            CreatePassworHash(password,out passwordHash, out passwordsalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordsalt;
            await _context.Users.AddAsync(user );
            await _context.SaveChangesAsync();
            return user;
        }

       

        public async Task<User> LogIn(string username , string password){
                var user = await _context.Users.FirstOrDefaultAsync(x=> x.UserName == username.ToLower());
                if( user == null)
                return null;
                if(!verifyPasswordHash(password, user.PasswordHash, user.PasswordSalt )){
                    return null;
                }
                return user;
          }

        public async Task<bool> UserExists(string username){
            if(await _context.Users.AnyAsync(x=> x.UserName ==  username))
                return true;

            return false;
         }  

          private void CreatePassworHash(string password, out byte[] passwordHash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt= hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

          private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
              var   computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
              for ( int i=0; i< computedHash.Length; i++){
                  if(computedHash[i] != passwordHash[i]) return false;
              }
            }    
            return true;
        }

    }
}