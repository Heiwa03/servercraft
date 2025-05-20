using Microsoft.EntityFrameworkCore;
using servercraft.Models.Domain;

namespace eUseControl.BusinessLogic
{
    public class BusinessLogicDbContext : DbContext
    {
        public BusinessLogicDbContext(DbContextOptions<BusinessLogicDbContext> options) : base(options)
        {
        }

        // DbSet properties for all entities
        public DbSet<Server> Servers { get; set; }
        public DbSet<ServerSpecification> ServerSpecifications { get; set; }
        public DbSet<ServerFullSpecs> ServerFullSpecs { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Server entity
            modelBuilder.Entity<Server>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            // Configure ServerSpecification entity
            modelBuilder.Entity<ServerSpecification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(s => s.Server)
                    .WithMany(s => s.Specifications)
                    .HasForeignKey(s => s.ServerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ServerFullSpecs entity
            modelBuilder.Entity<ServerFullSpecs>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(s => s.Server)
                    .WithOne(s => s.FullSpecs)
                    .HasForeignKey<ServerFullSpecs>(s => s.ServerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            });

            // Configure UserRole entity (many-to-many relationship)
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure CartItem entity
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CartId).IsRequired();
                entity.Property(e => e.ServerId).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.DateCreated).IsRequired();

                entity.HasOne(c => c.Server)
                    .WithMany()
                    .HasForeignKey(c => c.ServerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection",
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
            }
        }
    }
} 