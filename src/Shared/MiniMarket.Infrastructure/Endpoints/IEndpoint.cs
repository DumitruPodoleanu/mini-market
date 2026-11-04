using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Infrastructure.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}