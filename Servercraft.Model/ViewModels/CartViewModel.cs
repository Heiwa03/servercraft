// Models/ViewModels/CartViewModel.cs
using System.Collections.Generic;

namespace Servercraft.Model.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        public CartViewModel()
        {
            Items = new List<CartItemViewModel>();
        }
    }

    public class CartItemViewModel
    {
        public string ServerId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}