
using Ordering.Application.Orders.Queries.GetOrdersByName;
using System.Formats.Asn1;

namespace Ordering.API.EndPoints
{

    // public record GetOrdersByNameRequest(string Name);
    public record GetOrdersByNameResponse(List<OrderDto> Orders);

    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var query = new GetOrdersByNameQuery(orderName);
                var getOrdersResult = await sender.Send(query);
                var response = getOrdersResult.Adapt<GetOrdersByNameResponse>();
                return Results.Ok(response);

            }).Produces(StatusCodes.Status200OK)
            .WithName("GetOrdersByName")
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("GetOrdersByName")
            .WithDescription("GetOrdersByName");
            
        }
    }
}
