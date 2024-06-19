using Microsoft.AspNetCore.Authorization;
using Product.API.Products.CreateProduct;

namespace Product.API.Products.GetProductAndCustomerDetails;

public record GetProductAndCustomerDetailsRequest(string CustomerEmail);
public record GetProductAndCustomerDetailsResponse(UserProducts Product);

[Authorize(Roles = "Customer, Administrator")]
public class GetProductAndCustomerDetailsEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products/user_customer",
        [Authorize(Roles = "Customer, Administrator")]
        async (GetProductAndCustomerDetailsRequest request, ISender sender) =>
        {
            var command = request.Adapt<GetProductAndCustomerDetailsQuery>();

            var result = await sender.Send(command);

            var response = result.Adapt<GetProductAndCustomerDetailsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductAndCustomerDetails")
        .Produces<GetProductAndCustomerDetailsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Product And Customer Details")
        .WithDescription("Get Product And Customer Details");
    }
}
