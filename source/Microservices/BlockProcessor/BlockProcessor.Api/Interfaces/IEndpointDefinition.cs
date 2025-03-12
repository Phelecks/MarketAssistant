namespace BlockProcessor.Api.Interfaces;

public interface IEndpointDefinition
{
    void RegisterEndpoints(WebApplication app);
}