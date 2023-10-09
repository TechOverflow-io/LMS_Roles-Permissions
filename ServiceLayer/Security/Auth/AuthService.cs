using DataModelLayer.Configuration;
using DataModelLayer.Data;
using DataModelLayer.Enums;
using DataModelLayer.Models.DbModels;
using DataModelLayer.Models.DTOs.Request;
using DataModelLayer.Models.DTOs.Response.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Security.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApiDbContext _context;
        private readonly JwtConfig _jwtConfig;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor, ApiDbContext context)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _context = context;
        }
        public async Task<LoginLogicResponse> Login(UserLoginRequest user)
        {
            try
            {
                var existingUser = await _userManager.Users.Where(c => c.UserName == user.Username && !c.IsDeleted).FirstOrDefaultAsync();

                if (existingUser == null)
                {
                    return new LoginLogicResponse()
                    {
                        Message = "Invalid username or password",
                        IsSuccess = false,
                        MessageType = ResponseMessageType.BadRequest
                    };
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return new LoginLogicResponse()
                    {
                        Message = "Invalid username or password",
                        IsSuccess = false,
                        MessageType = ResponseMessageType.BadRequest
                    };

                }

                var jwtToken = GenerateAppToken(existingUser);

                var loginResponse = new LoginResponse()
                {
                    Success = true,
                    AccessToken = jwtToken,
                    Username = user.Username,
                    UserId = existingUser.Id,
                    Name = existingUser.FirstName + ' ' + existingUser.LastName,
                };

                return new LoginLogicResponse()
                {
                    Message = "",
                    IsSuccess = true,
                    LoginInfo = loginResponse
                };

            }
            catch (Exception ex)
            {
                return new LoginLogicResponse()
                {
                    Message = "Invalid request " + ex.Message,
                    IsSuccess = false,
                    MessageType = ResponseMessageType.BadRequest
                };

            }
        }

        private string GenerateAppToken(ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var userRolesForUser = _context.UserRoles.Include(i => i.Role).Where(o => o.UserId == user.Id);

            var userRoles = string.Join(",", userRolesForUser.Select(s => s.RoleId));

        
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim("Id", user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("accval", userRoles ), // All roles in that user
        }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

    }
}
