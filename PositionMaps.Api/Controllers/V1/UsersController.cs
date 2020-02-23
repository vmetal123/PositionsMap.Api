using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositionMaps.Api.Identity;

namespace PositionMaps.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UsersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userManager.Users.OrderBy(x => x.LastName).ToListAsync();
            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var appUser = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(appUser);

            return Ok(new
            {
                id = appUser.Id,
                firstName = appUser.FirstName,
                lastName = appUser.LastName,
                email = appUser.Email,
                roles = roles.ToArray()
            });
        }
    }
}