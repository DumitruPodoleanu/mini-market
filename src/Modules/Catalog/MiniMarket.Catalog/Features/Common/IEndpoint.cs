using Microsoft.AspNetCore.Routing;

namespace MiniMarket.Catalog.Features.Common;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}