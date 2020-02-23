using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PositionMaps.Api.Contracts.V1.Requests;
using PositionMaps.Api.Contracts.V1.Responses;
using PositionMaps.Api.Services;

namespace PositionMaps.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            var authResponse = await _identityService.RegisterAsync(request);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                UserId = authResponse.UserId,
                Success = authResponse.Success
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
                UserId = authResponse.UserId,
                Success = authResponse.Success
            });
        }

        [HttpGet("validateEmail")]
        [Authorize]
        public async Task<IActionResult> ValidateEmail(string email)
        {
            var result = await _identityService.ValidateEmail(email);

            return Ok(new { exists = result });
        }

        [HttpPost("update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var result = await _identityService.UpdateAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("resetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _identityService.ResetPasswordAsync(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}