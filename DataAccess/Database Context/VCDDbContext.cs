using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Database_Context
{
    public class VCDDbContext : DbContext
    {
        public VCDDbContext(DbContextOptions<VCDDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
    }
}
