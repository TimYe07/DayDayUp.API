using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DayDayUp.AccountContext
{
    public class UserService : IUserService
    {
        public UserService(IOptions<JwtSettings> jwtOptions, IOptions<Account> accountOptions)
        {
            _jwtSettings = jwtOptions.Value;
            _account = accountOptions.Value;
        }

        private readonly JwtSettings _jwtSettings;
        private readonly Account _account;

        public JwtToken Authenticate(string email, string password)
        {
            if (_account.Email != email || _account.Password != password)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, _account.Email),
                    new Claim(ClaimTypes.Name, _account.Name)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var ts = tokenDescriptor.Expires - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return new JwtToken
            {
                expires_in = Convert.ToInt64(ts?.TotalMilliseconds).ToString(),
                access_token = tokenHandler.WriteToken(token),
                token_type = "Bearer"
            };
        }

        public UserDto GetUser(string name)
        {
            if (_account.Name != name)
            {
                return null;
            }

            return new UserDto()
            {
                Name = _account.Name,
                Email = _account.Email,
                Avatar = _account.Avatar,
                Summary = _account.Summary
            };
        }
    }
}