using DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace VacationDaysCalculatorWebAPI.DatabaseContext
{
    public class VCDDbContext : DbContext
    {
        public VCDDbContext(DbContextOptions<VCDDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
