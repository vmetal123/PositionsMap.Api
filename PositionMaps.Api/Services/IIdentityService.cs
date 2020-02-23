using PositionMaps.Api.Contracts.V1.Requests;
using PositionMaps.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<bool> ValidateEmail(string email);
        Task<AuthenticationResult> UpdateAsync(UserUpdateRequest request);
        Task<AuthenticationResult> ResetPasswordAsync(ChangePasswordRequest request);
    }
}
