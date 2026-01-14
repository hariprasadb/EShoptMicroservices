using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.EndPoints
{
    //--Accpets a CreateOrderRequest object
    //--Maps the request to a CreateOrderCommand
    //--Uses MediatR to Send the command to the corresponding handler
    //--return a response with created Order's ID

    public record CreateOrderRequest(OrderDto Order);
    public record CreateOrderResponse(Guid Id);
    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateOrderResponse>();
                return Results.Created($"/orders/{response.Id}", response);

            })
            .WithName("CreateOrder")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Create Order")
            .WithSummary("CreateOrder");
        }
    }
}
