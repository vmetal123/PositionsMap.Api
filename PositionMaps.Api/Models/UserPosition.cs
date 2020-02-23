using PositionMaps.Api.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Models
{
    public class UserPosition
    {
        [Key]
        public string Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public DateTimeOffset Date { get; set; }
        public AppUser User { get; set; }
        public string UserId { get; set; }
    }
}
