using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servercraft.Domain.Entities
{
    public class ServerFullSpecs
    {
        [Key]
        [ForeignKey("Server")]
        public string ServerId { get; set; }

        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Storage { get; set; }
        public string Network { get; set; }
        public string Power { get; set; }
        public string FormFactor { get; set; }

        public virtual Server Server { get; set; }
    }
} 