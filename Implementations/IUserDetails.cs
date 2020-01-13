using Data;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserDetails : IAsyncRepository<UserDetails>
    {
        Task<UserDetails> Register(UserDetails userDetails, string password);

        Task<bool> UserExists(string emailID);

        Task<UserDetails> Login(string email, string password);
    }
}
