using System.Reflection;
using BaseApi.Interfaces;

namespace BaseApi.Extensions;

public static class MinimalApiExtension
{
    public static void RegisterEndpointDefinitions(this WebApplication app)
    {
        // You can replcae with typeof(Program).Assembly
        var endpointDefinitions = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IEndpointDefinition)) && !t.IsAbstract
            && !t.IsInterface)
            .Select(Activator.CreateInstance).Cast<IEndpointDefinition>();

        foreach (var endpointdef in endpointDefinitions)
            endpointdef.RegisterEndpoints(app);
        
    }
}