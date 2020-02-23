using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PositionMaps.Api.Identity
{
    public class IdentityAppContext: IdentityDbContext<AppUser, AppRole, string>
    {
        public IdentityAppContext(DbContextOptions<IdentityAppContext> options): base(options)
        {

        }
    }
}
