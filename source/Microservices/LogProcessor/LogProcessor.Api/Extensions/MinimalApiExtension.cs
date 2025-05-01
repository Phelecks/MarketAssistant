using LogProcessor.Api.Interfaces;

namespace LogProcessor.Api.Extensions;

public static class MinimalApiExtension
{
    public static void RegisterEndpointDefinitions(this WebApplication app)
    {
        var endpointDefinitions = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEndpointDefinition)) && !t.IsAbstract
            && !t.IsInterface)
            .Select(Activator.CreateInstance).Cast<IEndpointDefinition>();

        foreach (var endpointdef in endpointDefinitions)
            endpointdef.RegisterEndpoints(app);
        
    }
}