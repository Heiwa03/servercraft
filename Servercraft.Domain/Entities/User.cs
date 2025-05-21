using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servercraft.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(256)]
        public string ProfilePictureUrl { get; set; }

        [StringLength(100)]
        public string AddressStreet { get; set; }

        [StringLength(50)]
        public string AddressCity { get; set; }

        [StringLength(50)]
        public string AddressCountry { get; set; }

        [StringLength(20)]
        public string AddressPostalCode { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public User()
        {
            UserRoles = new HashSet<UserRole>();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }

    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }
    }

    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
} 