using PositionMaps.Api.Contracts.V1.Requests;
using PositionMaps.Api.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Services
{
    public interface ILocationService
    {
        Task<LocationSuccessResponse> AddAsync(UserLocationRequest request);
    }
}
