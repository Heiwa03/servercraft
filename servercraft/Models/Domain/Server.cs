// Models/Domain/Server.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace servercraft.Models.Domain
{
    public class Server
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        [StringLength(50)]
        public string Badge { get; set; }

        public bool InStock { get; set; }

        public virtual ICollection<ServerSpecification> Specifications { get; set; }

        public virtual ServerFullSpecs FullSpecs { get; set; }

        public Server()
        {
            Specifications = new List<ServerSpecification>();
        }
    }

    public class ServerSpecification
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public string ServerId { get; set; }

        public virtual Server Server { get; set; }
    }

    public class ServerFullSpecs
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Processor { get; set; }

        [StringLength(100)]
        public string Memory { get; set; }

        [StringLength(100)]
        public string Storage { get; set; }

        [StringLength(100)]
        public string Network { get; set; }

        [StringLength(100)]
        public string Power { get; set; }

        [StringLength(50)]
        public string FormFactor { get; set; }

        public string ServerId { get; set; }

        public virtual Server Server { get; set; }
    }
}