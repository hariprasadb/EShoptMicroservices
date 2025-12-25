namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasket(string userName, CancellationToken token=default);
        Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken token=default);

        Task<bool> DeleteBasket(string userName, CancellationToken token=default);
    }
}
