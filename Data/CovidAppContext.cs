using Covid_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Covid_Api.Data
{
    public class CovidAppContext : DbContext
    {
        public CovidAppContext(DbContextOptions<CovidAppContext> opt) : base(opt)
        {
            // Database.EnsureCreated();
        }
        public DbSet<DailyData> dailyDatas { get; set; }

        public DbSet<Country> countries { get; set; }

        public DbSet<Log> logs { get; set; }
    }

}
