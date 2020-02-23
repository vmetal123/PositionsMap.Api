using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Contracts.V1.Requests
{
    public class ChangePasswordRequest
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}
