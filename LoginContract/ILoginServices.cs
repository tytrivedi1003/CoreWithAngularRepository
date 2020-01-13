using DTOs;

namespace LoginContract
{
    public interface ILoginServices
    {
        string GenerateJSONWebToken(UserDetails userModel);
        UserModel Authenticate(UserDetails userModel);
    }
}
