using BuildingBlocks.Pagination;
using System.Collections;
using MediatR;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.EndPoints
{

    //Accepts Pagination parameters
    //constructs a GetOrderQuery with these parameters
    //Retrieves the data and returns it in a paginated format

    //public record GetOrdersRequest(int PageIndex, int PageSize);
    public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
    public class GetOrders : ICarterModule

    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters] PaginationRequest request,
                ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(request));
                var ordersResponse = result.Adapt<GetOrdersResponse>();
                return Results.Ok(ordersResponse.Orders);
            }).Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetOrders")
            .WithSummary("Get Orders by Pages")
            .WithDescription("Get Orders");
        }
    }
}
