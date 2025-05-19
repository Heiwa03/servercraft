using System.Collections.Generic;

namespace servercraft.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public BankAccountDetails BankAccount { get; set; }

        public CheckoutViewModel()
        {
            Items = new List<CartItemViewModel>();
            BankAccount = new BankAccountDetails();
        }
    }

    public class BankAccountDetails
    {
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
    }

    public enum PaymentMethod
    {
        BankAccount,
        GooglePay,
        PayPal
    }
} 