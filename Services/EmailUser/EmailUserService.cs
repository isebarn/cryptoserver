using System;
using Microsoft.Extensions.Options;
using Helpers;
using Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public interface IEmailUserService
    {
        User Authenticate(string username, string password);
        User Create(EmailUser user, string password);
    }

    public class EmailUserService : IEmailUserService
    {

        private readonly AppSettings _appSettings;
        private Context _context;

        public EmailUserService(Context context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public User Create(EmailUser user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Password is required");

            if (_context.EmailUsers.Any(x => x.Username == user.Username))
                throw new Exception("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            PasswordHashing.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.User = new User();

            _context.EmailUsers.Add(user);
            _context.SaveChanges();

            return user.User;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var emailUser = _context.EmailUsers.Include(x => x.User).FirstOrDefault(x => x.Username == username);

            // check if username exists
            if (emailUser == null)
                return null;

            // check if password is correct
            if (!PasswordHashing.VerifyPasswordHash(password, emailUser.PasswordHash, emailUser.PasswordSalt))
                return null;

            // authentication successful
            return emailUser.User;
        }      
    }
}