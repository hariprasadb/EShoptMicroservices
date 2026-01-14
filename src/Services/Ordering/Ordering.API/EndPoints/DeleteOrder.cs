
using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.EndPoints
{

    //Accepts the order ID as a parameter
    // constructs a deleteOrderCommand
    // Sends the command using MediatR.
    //Returns a success or not found response.
    public record DeleteOrderResponse(bool IsSuccess);
    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{Id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(Id));
                var response = result.Adapt<DeleteOrderResponse>();

                return Results.Ok(response);

            }).WithName("DeleteOrder")
            .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete order")
            .WithDescription("Delete ORder");
        }
    }
}
