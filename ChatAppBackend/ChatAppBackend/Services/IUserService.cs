using ChatAppBackend.Controllers;

namespace ChatAppBackend.Services;

public interface IUserService
{
    Task<string> GetToken(LoginModel login);
    Task<bool> RegisterUser(RegisterModel model);

}