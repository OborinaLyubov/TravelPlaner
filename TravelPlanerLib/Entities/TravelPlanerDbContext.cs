using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TravelPlanerLib.Entities
{
    public class TravelPlanerDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public TravelPlanerDbContext(DbContextOptions<TravelPlanerDbContext> options) : base(options) { }
    }
}
