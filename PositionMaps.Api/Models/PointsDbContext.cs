using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Models
{
    public class PointsDbContext: DbContext
    {
        public PointsDbContext(DbContextOptions<PointsDbContext> options): base(options)
        {

        }

        public DbSet<UserPosition> UserPositions { get; set; }
    }
}
