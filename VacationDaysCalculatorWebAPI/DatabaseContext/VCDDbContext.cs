using DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace VacationDaysCalculatorWebAPI.DatabaseContext
{
    public class VCDDbContext : DbContext
    {
        public VCDDbContext(DbContextOptions<VCDDbContext> options) : base(options) { }

        public DbSet<VacationDays> VacationDays { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<RemainingVacationDays> RemainingVacationDays { get; set; }
        public DbSet<Weekend> Weekends { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
