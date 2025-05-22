using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servercraft.Domain.Entities
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual Servercraft.Domain.Entities.User User { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Servercraft.Domain.Entities.Role Role { get; set; }
    }
} 