using Microsoft.AspNetCore.Identity;
using PositionMaps.Api.Domain;
using PositionMaps.Api.Identity;
using PositionMaps.Api.Options;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using PositionMaps.Api.Contracts.V1.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;

namespace PositionMaps.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(JwtSettings jwtSettings, UserManager<AppUser> userManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult { Errors = new[] { "User does not exist" } };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Wrong password" }
                };
            }

            return GenerateAuthenticationResultForUser(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult { Errors = new[] { "User with this email exists" } };
            }

            var newUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            foreach (var role in request.Roles)
            {
                var rolesAdded = await _userManager.AddToRoleAsync(newUser, role.Name);
            }

            return GenerateAuthenticationResultForUser(newUser);
        }

        public async Task<bool> ValidateEmail(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);

            if (result == null)
            {
                return false;
            }

            return true;

        }

        public async Task<AuthenticationResult> UpdateAsync(UserUpdateRequest request)
        {
            var appUser = await _userManager.FindByIdAsync(request.Id);

            if (appUser == null)
            {
                return new AuthenticationResult { Errors = new[] { "Uset not found" }, Success = false };
            }

            var roles = await _userManager.GetRolesAsync(appUser);

            if (roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(appUser, role);
                }
            }

            foreach (var role in request.Roles)
            {
                await _userManager.AddToRoleAsync(appUser, role.Name);
            }

            return new AuthenticationResult { Success = true, UserId = appUser.Id };
        }

        public async Task<AuthenticationResult> ResetPasswordAsync(ChangePasswordRequest request)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(request.Id);

                string token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

                var result = await _userManager.ResetPasswordAsync(appUser, token, request.Password);

                if (!result.Succeeded)
                {
                    return new AuthenticationResult { Errors = result.Errors.Select(x => x.Description), Success = false };
                }
            }
            catch (Exception e)
            {
                return new AuthenticationResult { Errors = new List<string> { e.Message.ToString() }, Success = false };
            }
            return new AuthenticationResult { Success = true };
        }

        private AuthenticationResult GenerateAuthenticationResultForUser(AppUser user)
        {
            string errorMessage = string.Empty;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new AuthenticationResult
                {
                    Success = true,
                    Token = tokenHandler.WriteToken(token),
                    UserId = user.Id
                };
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message.ToString();
            }

            return new AuthenticationResult
            {
                Errors = new[] { errorMessage }
            };
        }
    }
}
