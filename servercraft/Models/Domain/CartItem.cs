// Models/Domain/CartItem.cs
namespace servercraft.Models.Domain
{
    public class CartItem
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public string ServerId { get; set; }
        public int Quantity { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual Server Server { get; set; }
    }
}