using Basket.API.Data;

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketQuery(string UserName):IQuery<GetBasketResult>;

    public record GetBasketResult(ShoppingCart Cart);

    public class GetBasketHandler(IBasketRepository repository) : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        public async Task<GetBasketResult> Handle(GetBasketQuery query, 
                                                 CancellationToken cancellationToken)
        {
            //ToDo: Get Basket from database
            //var basket = await _repository.GetBasket(request.UserName);
            var shoppingCart = await repository.GetBasket(query.UserName);
            return new GetBasketResult(shoppingCart);
        }
    }
}
