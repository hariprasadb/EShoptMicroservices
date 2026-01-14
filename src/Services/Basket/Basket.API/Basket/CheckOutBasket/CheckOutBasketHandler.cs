
using Basket.API.Data;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckOutBasket
{
     public record CheckOutBasketCommand(BasketCheckOutDto BasketCheckDto)
         :ICommand<CheckOutBasketResult>;
     public record CheckOutBasketResult(bool IsSuccess);

    public class CheckOutBasketCommandValidator :
        AbstractValidator<CheckOutBasketCommand>
    {
        public CheckOutBasketCommandValidator()
        {
            RuleFor(O => O.BasketCheckDto).NotNull().WithMessage("Basket Checkout Cannot be null/empty");
            RuleFor(O => O.BasketCheckDto.UserName).NotEmpty().WithMessage("User name is required");

        }
    }
    public class CheckOutBasketHandler(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint  )
        : ICommandHandler<CheckOutBasketCommand, CheckOutBasketResult>
    {
        public async Task<CheckOutBasketResult> Handle(CheckOutBasketCommand command, CancellationToken cancellationToken)
        {
            //get existing basket with total price
            //Set totalPrice on basketcheckOut event message 
            //send basket checkout event to rabbitmq using masstransit
            //delete the basket 
           var basket=  await basketRepository.GetBasket(command.BasketCheckDto.UserName, cancellationToken);
            if (basket == null)
            {
                return new CheckOutBasketResult(false);
            }
            var eventMessage = command.BasketCheckDto.Adapt<BasketCheckoutEvent>();
            eventMessage.TotalPrice = basket.TotalPrice;    
            await publishEndpoint.Publish(eventMessage, cancellationToken);
            await basketRepository.DeleteBasket(command.BasketCheckDto.UserName,
                  cancellationToken
                );
            return new CheckOutBasketResult(true);

        }
    }
}
