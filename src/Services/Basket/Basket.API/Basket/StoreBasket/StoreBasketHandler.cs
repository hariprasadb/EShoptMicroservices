

using Basket.API.Data;
using Dicount.Grpc;

namespace Basket.API.Basket.StoreBasket
{

    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator() {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("User Name is required");
        }
    }
    public class StoreBasketCommandHandler(IBasketRepository repository,
         DiscountProtoService.DiscountProtoServiceClient discountProto

        ) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.Cart;

            await DeductDiscount(cart);
             // use Marten Upsert - if exist update, if not insert
             await repository.StoreBasket(cart);

            return new StoreBasketResult(cart.UserName);
            
        }
        private async Task DeductDiscount(ShoppingCart cart)
        {
            foreach (var item in cart.Items)
            {
                var discountRequest = new GetDiscountRequest()
                { ProductName = item.ProductName };

                var coupon = await discountProto.GetDiscountAsync(discountRequest);
                item.Price -= coupon.Amount;
            }

        }
    }
}
