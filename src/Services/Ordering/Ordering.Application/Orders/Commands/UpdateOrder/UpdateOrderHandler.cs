
namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDBContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {

             //Update the order entity from command object
             //Save to database
             //return the result
             var orderId = OrderId.Of(command.Order.Id);
            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
            if (order == null)
            {
                throw new OrderNotFoundExceptions(command.Order.Id);

            }   
            UpdateOrderwithNewValues(order,command.Order);
            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new UpdateOrderResult(true);
        }
        private void UpdateOrderwithNewValues(Order order,OrderDto orderDto)
        {

            var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName,
                                             orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine,
                                             orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State,
                                             orderDto.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName,
                                          orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine,
                                          orderDto.BillingAddress.Country, orderDto.BillingAddress.State,
                                          orderDto.BillingAddress.ZipCode);
            order.Update(orderName: OrderName.Of(orderDto.OrderName),
                         shippingAddress: shippingAddress,
                         billingAddress: billingAddress,
                         payment: Payment.Of(orderDto.Payment.CardName,
                         orderDto.Payment.CardNumber,
                         orderDto.Payment.Expiration, orderDto.Payment.Cvv,
                         orderDto.Payment.PaymentMethod),
                         orderDto.Status
                         );
        }
    }
}
