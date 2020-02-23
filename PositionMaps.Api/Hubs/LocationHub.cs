using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PositionMaps.Api.Contracts.V1.Requests;
using PositionMaps.Api.Models;
using PositionMaps.Api.Services;
using System;
using System.Threading.Tasks;

namespace PositionMaps.Api.Hubs
{
    [Authorize]
    public class LocationHub: Hub
    {
        private readonly ILocationService _locationService;

        public LocationHub(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public override Task OnConnectedAsync()
        {
            var user = Context.User;

            if (user.IsInRole("admin"))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var user = Context.User;

            if (user.IsInRole("admin"))
            {
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "admin");
            }
            
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message)
        {
            try
            {
                var jsonMessage = JsonConvert.DeserializeObject<UserLocationRequest>(message);

                //var result = await _locationService.AddAsync(jsonMessage);

                //if (!result.Success)
                //{
                //    await Clients.User(user).SendAsync("errorMessage", "An error ocurred");
                //}
                //else
                //{
                //    await Clients.User(user).SendAsync("message", jsonMessage);
                //}

                await Clients.Groups("admin").SendAsync("locationUpdate", jsonMessage);
            }
            catch (Exception ex)
            {
                await Clients.User(user).SendAsync("errorMessage", "An error ocurred");
            }
            
        }
    }
}
