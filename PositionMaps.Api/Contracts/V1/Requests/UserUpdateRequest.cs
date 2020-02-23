using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Contracts.V1.Requests
{
    public class UserUpdateRequest
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}
