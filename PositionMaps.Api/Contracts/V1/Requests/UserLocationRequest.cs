using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Contracts.V1.Requests
{
    public class UserLocationRequest
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string UserId { get; set; }
    }
}
