using Microsoft.EntityFrameworkCore;
using servercraft.Models.Domain;

namespace eUseControl.BusinessLogic
{
    public class BusinessLogicDbContext : DbContext
    {
        public BusinessLogicDbContext(DbContextOptions<BusinessLogicDbContext> options) : base(options)
        {
        }

        public DbSet<Server> Servers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Add any additional configuration here if needed
            base.OnModelCreating(modelBuilder);
        }
    }
} 