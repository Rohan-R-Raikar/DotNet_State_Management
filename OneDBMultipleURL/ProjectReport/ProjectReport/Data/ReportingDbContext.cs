using Microsoft.EntityFrameworkCore;
using ProjectReport.Models;

namespace ProjectReport.Data
{
    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext(DbContextOptions<ReportingDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}
