using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrdersByName
{
    public class GetOrdersByNameHandler(IApplicationDBContext dbContext)
        : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
    {
        public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query,
                                                  CancellationToken cancellationToken)
        {
            //get orders by name using dbcontext
            //return result
            var orders = await dbContext.Orders.Include(O => O.OrderItems)
                                .AsNoTracking()
                                .Where(O => O.OrderName.Value.Contains(query.Name))
                                .OrderBy(O => O.OrderName.Value)
                                .ToListAsync(cancellationToken);
            var orderDtos = orders.ToOrderDtoList();
            return new GetOrdersByNameResult(orderDtos);
        }

        
    }
}
