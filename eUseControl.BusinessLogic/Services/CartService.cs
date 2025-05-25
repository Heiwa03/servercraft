using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUseControl.Domain.Models;
using eUseControl.Domain.Models.Domain;
using eUseControl.Domain.Models.ViewModels;
using eUseControl.Domain.Interfaces.Services;
using servercraft.Models.Repositories;

namespace eUseControl.BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CartViewModel> GetCartAsync(string cartId)
        {
            var cartItems = await _unitOfWork.CartItems.FindAsync(c => c.CartId == cartId);
            var viewModel = new CartViewModel
            {
                Items = cartItems.Select(item => new CartItemViewModel
                {
                    ServerId = item.ServerId,
                    Name = item.Server.Name,
                    Price = item.Server.Price,
                    Quantity = item.Quantity
                }).ToList()
            };

            viewModel.Subtotal = viewModel.Items.Sum(item => item.Price * item.Quantity);
            viewModel.Shipping = viewModel.Subtotal > 0 ? 10 : 0;
            viewModel.Tax = viewModel.Subtotal * 0.2m;
            viewModel.Total = viewModel.Subtotal + viewModel.Shipping + viewModel.Tax;

            return viewModel;
        }

        public async Task<bool> AddToCartAsync(string cartId, string serverId)
        {
            var cartItem = await _unitOfWork.CartItems.FindAsync(c => c.CartId == cartId && c.ServerId == serverId);
            if (cartItem.Any())
            {
                var item = cartItem.First();
                item.Quantity++;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            var server = await _unitOfWork.Servers.GetByIdAsync(serverId);
            if (server == null)
            {
                return false;
            }

            var newCartItem = new CartItem
            {
                CartId = cartId,
                ServerId = serverId,
                Quantity = 1,
                DateCreated = DateTime.UtcNow
            };

            _unitOfWork.CartItems.Add(newCartItem);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuantityAsync(string cartId, string serverId, int quantity)
        {
            var cartItem = await _unitOfWork.CartItems.FindAsync(c => c.CartId == cartId && c.ServerId == serverId);
            if (!cartItem.Any())
            {
                return false;
            }

            var item = cartItem.First();
            if (quantity <= 0)
            {
                _unitOfWork.CartItems.Delete(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(string cartId, string serverId)
        {
            var cartItem = await _unitOfWork.CartItems.FindAsync(c => c.CartId == cartId && c.ServerId == serverId);
            if (!cartItem.Any())
            {
                return false;
            }

            _unitOfWork.CartItems.Delete(cartItem.First());
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCartBatchAsync(string cartId, List<CartUpdateItem> updates)
        {
            foreach (var update in updates)
            {
                await UpdateQuantityAsync(cartId, update.Id, update.Quantity);
            }
            return true;
        }

        public async Task<decimal> GetCartTotalAsync(string cartId)
        {
            var cart = await GetCartAsync(cartId);
            return cart.Total;
        }

        public async Task<int> GetCartCountAsync(string cartId)
        {
            var cartItems = await _unitOfWork.CartItems.FindAsync(c => c.CartId == cartId);
            return cartItems.Sum(item => item.Quantity);
        }
    }
} 