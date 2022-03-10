using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ChatAppBackend.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatAppBackend.Services
{
    public class UserService : IUserService
    {
        private readonly JwtSettings _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserService(IOptions<JwtSettings> configuration, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _configuration = configuration.Value;
        }

        public async Task<string> GetToken(LoginModel login)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(login.Email) ??
                        await _signInManager.UserManager.FindByNameAsync(login.Email);

            if (user is null) return string.Empty;

            var loginResult = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);

            if (!loginResult.Succeeded) return string.Empty;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration.Issuer,
                _configuration.Audience,
                null,
                expires: DateTime.UtcNow.AddDays(_configuration.ExpiryInDays),
                signingCredentials: credentials);

            var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return serializedToken;
        }

        public async Task<bool> RegisterUser(RegisterModel model)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email) ??
                       await _signInManager.UserManager.FindByNameAsync(model.Username);

            if (user is not null) return false;

            var creationResult = await _signInManager.UserManager.CreateAsync(new IdentityUser
            {
                Email = model.Email,
                UserName = model.Username
            }, model.Password);

            return creationResult.Succeeded;
        }
    }

    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string SecurityKey { get; set; }
        public int ExpiryInDays { get; set; }
        public string Audience { get; set; }
    }
}
