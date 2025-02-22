using Microsoft.EntityFrameworkCore;
using SampleCoreAPIApp.Models;

namespace SampleCoreAPIApp.DBContext
{
    public class SampleTempDBContext : DbContext
    {
        public SampleTempDBContext(DbContextOptions<SampleTempDBContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
