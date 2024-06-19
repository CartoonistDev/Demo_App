using Microsoft.AspNetCore.Authorization;

namespace Product.API.Products.GetProductById;

//public record GetProductByIdRequest(Guid id);
public record GetProductByIdResponse(OrderProduct Product);

[Authorize(Roles = "Customer, Administrator")]
public class GetProductByIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/id/{id}",
        [Authorize(Roles = "Customer, Administrator")]
        async ([FromQuery]Guid id ,ISender sender) =>
        {

            var result = await sender.Send(new GetProductByIdQuery(id));

            var response = result.Adapt<GetProductByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductById")
        .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product By Id")
        .WithDescription("Get Product By Id");
    }
}
