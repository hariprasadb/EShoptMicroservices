using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer
{
    internal class GetOrdersByCustomerHandler(IApplicationDBContext dbContext) 
        : IQueryHandler<GetOrdersByCustomerQuery,
                            GetOrdersByCustomerResult>
    {
        public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
        {
            //Get Orders by customer using dbContext
            //return result
            var orders= await dbContext.Orders.
                            Include(O=>O.OrderItems)
                            .AsNoTracking()
                            .Where(O => O.CustomerId == CustomerId.Of(request.CustomerId))
                            .OrderBy(O => O.OrderName.Value)
                            .ToListAsync();

            return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
        }
    }
}
