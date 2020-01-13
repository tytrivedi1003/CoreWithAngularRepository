using Data;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace Implementations
{
    public class UserDetailsRepository : EfRepository<UserDetails>, IUserDetails
    {
        private readonly DevDbContext _dbContext;
        public UserDetailsRepository(DevDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDetails> Register(UserDetails user, string password)
        {
            try
            {
                byte[] passwordHash, salt;
                CreatePasswordHash(password, out passwordHash, out salt);
                user.Password = passwordHash;
                user.Salt = salt;

                await _dbContext.UserDetails.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<UserDetails> Login(string email, string password)
        {
            var user = await _dbContext.UserDetails.FirstOrDefaultAsync(x => x.EmailId == email);
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.Password, user.Salt))
                return null;

            return user; // auth successful
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] salt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<bool> UserExists(string emailID)
        {
            if (await _dbContext.UserDetails.AnyAsync(x => x.EmailId == emailID))
                return true;
            return false;
        }
    }
}
