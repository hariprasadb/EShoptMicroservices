
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Threading;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository repository, 
        IDistributedCache cache) : IBasketRepository
    {
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            
            await repository.DeleteBasket(userName, cancellationToken);
            cache.Remove(userName);
            return true;

        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            }
            var shoppingCart= await repository.GetBasket(userName, cancellationToken);
            cachedBasket = JsonSerializer.Serialize(shoppingCart);
            await cache.SetStringAsync(userName, cachedBasket);
            return shoppingCart;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            var cachedBasket = JsonSerializer.Serialize(cart);
            await cache.SetStringAsync(cart.UserName, cachedBasket);
            return await repository.StoreBasket(cart, cancellationToken);

        }
    }
}
