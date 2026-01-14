using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrders
{
    public class GetOrdersHandler(IApplicationDBContext dbContext)
        : IQueryHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            var pageIndex = query.PaginationRequest.PageIndex;
            var pageSize = query.PaginationRequest.PageSize;

            var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);
            var orders = await dbContext.Orders.Include(O => O.OrderItems)
                            .OrderBy(O => O.OrderName.Value)
                            .Skip(pageSize * pageIndex)
                            .Take(pageSize).ToListAsync();
            var orderDtos = orders.ToOrderDtoList();
            var pageinatedResult = new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orderDtos);
            return new GetOrdersResult(pageinatedResult);



        }
    }
}
