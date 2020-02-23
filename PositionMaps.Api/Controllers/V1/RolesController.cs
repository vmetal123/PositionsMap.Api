using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PositionMaps.Api.Contracts.V1.Requests;
using PositionMaps.Api.Identity;

namespace PositionMaps.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreationRequest request)
        {
            var appRole = new AppRole { Id = Guid.NewGuid().ToString(), Name = request.Role };
            var result = await _roleManager.CreateAsync(appRole);

            if (result.Succeeded)
            {
                return Ok("Role created successfully");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error ocurred"});
        }
    }
}