using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.Common.Endpoints;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}