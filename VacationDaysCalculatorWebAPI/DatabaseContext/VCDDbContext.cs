using DomainModel.Models;
using Microsoft.EntityFrameworkCore;

namespace VacationDaysCalculatorWebAPI.DatabaseContext
{
    public class VacationDbContext : DbContext
    {
        public VacationDbContext(DbContextOptions<VacationDbContext> options) : base(options) { }

        public DbSet<Vacation> Vacation { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<RemainingVacation> RemainingVacation { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
