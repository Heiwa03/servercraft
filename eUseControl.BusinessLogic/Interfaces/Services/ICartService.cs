using System.Threading.Tasks;
using servercraft.Models.ViewModels;

namespace eUseControl.BusinessLogic.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartViewModel> GetCartAsync(string cartId);
        Task<bool> AddToCartAsync(string cartId, string serverId);
        Task<bool> UpdateQuantityAsync(string cartId, string serverId, int quantity);
        Task<bool> RemoveFromCartAsync(string cartId, string serverId);
        Task<bool> UpdateCartBatchAsync(string cartId, System.Collections.Generic.List<CartUpdateItem> updates);
        Task<decimal> GetCartTotalAsync(string cartId);
        Task<int> GetCartCountAsync(string cartId);
    }
} 