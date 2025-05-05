// Models/ServerMarketContext.cs
using System.Data.Entity;
using servercraft.Models.Domain;

namespace servercraft.Models
{
    public class ServerMarketContext : DbContext
    {
        public ServerMarketContext() : base("DefaultConnection")
        {
        }

        public DbSet<Server> Servers { get; set; }
        public DbSet<ServerSpecification> ServerSpecifications { get; set; }
        public DbSet<ServerFullSpecs> ServerFullSpecs { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure one-to-many relationship between Server and ServerSpecification
            modelBuilder.Entity<ServerSpecification>()
                .HasRequired(s => s.Server)
                .WithMany(s => s.Specifications)
                .HasForeignKey(s => s.ServerId);

            // Configure one-to-one relationship between Server and ServerFullSpecs
            modelBuilder.Entity<ServerFullSpecs>()
                .HasRequired(s => s.Server)
                .WithOptional(s => s.FullSpecs);

            // Configure many-to-many relationship between User and Role
            modelBuilder.Entity<UserRole>()
                .HasRequired(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasRequired(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}