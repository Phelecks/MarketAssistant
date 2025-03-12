using BlockProcessor.Api.Interfaces;
using BlockProcessor.Application.RpcUrl.Queries.GetRpcUrls;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlockProcessor.Api.MinimalAPIs;

public class RpcUrlEndpointsV1 : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet().HasApiVersion(new Asp.Versioning.ApiVersion(majorVersion: 1, minorVersion: 0)).ReportApiVersions().Build();
        
        var endpointGroup = app.MapGroup("/RpcUrls").WithTags("RpcUrls").WithApiVersionSet(apiVersionSet).MapToApiVersion(majorVersion: 1, minorVersion: 0).WithOpenApi();

        endpointGroup.MapGet("/", GetAsync);
    }

    async static Task<IResult> GetAsync(ISender sender, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? orderBy, CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new GetRpcUrlsQuery(bypassCache: false, cacheKey: "BlockProcessor_RpcUrls", expireInMinutes: 1) { PageNumber = pageNumber, PageSize = pageSize, OrderBy = orderBy}, cancellationToken));
    }
}