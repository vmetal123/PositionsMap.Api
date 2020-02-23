using Microsoft.AspNetCore.Http;
using PositionMaps.Api.Contracts.V1.Requests;
using PositionMaps.Api.Contracts.V1.Responses;
using PositionMaps.Api.Models;
using System;
using System.Threading.Tasks;

namespace PositionMaps.Api.Services
{
    public class LocationService: ILocationService
    {
        private readonly PointsDbContext _context;

        public LocationService(PointsDbContext context)
        {
            _context = context;
        }

        public async Task<LocationSuccessResponse> AddAsync(UserLocationRequest request)
        {
            try
            {
                var position = new UserPosition
                {
                    Id = Guid.NewGuid().ToString(),
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Date = DateTime.Now,
                    UserId = request.UserId
                };

                await _context.UserPositions.AddAsync(position);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new LocationSuccessResponse
                {
                    Success = false,
                    Errors = new string[] { ex.Message.ToString() }
                };
            }

            return new LocationSuccessResponse
            {
                Success = true
            };
        }
    }
}
