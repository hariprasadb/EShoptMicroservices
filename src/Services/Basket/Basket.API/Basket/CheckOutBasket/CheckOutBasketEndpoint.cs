namespace Basket.API.Basket.CheckOutBasket
{

    public record CheckOutBasketRequest (BasketCheckOutDto BasketCheckDto);
    public record CheckOutBasketResponse(bool IsSuccess);

    public class CheckOutBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/CheckOut", async (CheckOutBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<CheckOutBasketCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CheckOutBasketResponse>();

                return Results.Ok(response);
            })
            .WithName("CheckOutBaket")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("CheckOutBaket")
            .WithDescription("CheckOutBaket");

        }
    }
}
