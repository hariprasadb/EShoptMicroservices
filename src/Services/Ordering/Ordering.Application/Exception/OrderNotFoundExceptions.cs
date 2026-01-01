using BuildingBlocks.Exceptions;


namespace Ordering.Application.Exceptions
{
    public class OrderNotFoundExceptions: NotFoundException
    {
        public OrderNotFoundExceptions(Guid id):base("Order",id) 
        {
        }
       

    }
}
