using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PositionMaps.Api.Contracts.V1.Requests;
using PositionMaps.Api.Extensions;
using PositionMaps.Api.Hubs;
using PositionMaps.Api.Services;

namespace PositionMaps.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IHubContext<LocationHub> _hub;

        public LocationController(ILocationService locationService, IHubContext<LocationHub> hub)
        {
            _locationService = locationService;
            _hub = hub;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(UserLocationRequest request)
        {
            try
            {
                request.UserId = HttpContext.GetUserId();

                var result = await _locationService.AddAsync(request);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                var response = JsonConvert.SerializeObject(request);

                await _hub.Clients.Client(request.UserId).SendAsync("newLocation", response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();        
        }
    }
}