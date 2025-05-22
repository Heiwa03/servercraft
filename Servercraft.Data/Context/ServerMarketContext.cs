// Models/ServerMarketContext.cs
using System.Data.Entity;
using Servercraft.Domain.Entities;

namespace Servercraft.Data.Context
{
    public class ServerMarketContext : DbContext
    {
        public ServerMarketContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<ServerFullSpecs> ServerFullSpecs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithRequired(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            // Configure Role
            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithRequired(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            // Configure UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Configure Server
            modelBuilder.Entity<Server>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Server>()
                .HasOptional(s => s.FullSpecs)
                .WithRequired(s => s.Server)
                .WillCascadeOnDelete(true);

            // Configure ServerFullSpecs
            modelBuilder.Entity<ServerFullSpecs>()
                .HasKey(s => s.ServerId);

            // Configure CartItem
            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CartItem>()
                .HasRequired(c => c.Server)
                .WithMany()
                .HasForeignKey(c => c.ServerId);
        }
    }
}