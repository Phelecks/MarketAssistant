using Ocelot.Configuration;
using Ocelot.Logging;
using Ocelot.Responses;
using Ocelot.ServiceDiscovery;
using Ocelot.ServiceDiscovery.Providers;

namespace Application.Gateway.Helpers
{
    public class AspireServiceDiscoveryProviderFactory : IServiceDiscoveryProviderFactory
    {
        private readonly IOcelotLoggerFactory _factory;
        private readonly IServiceProvider _provider;

        public AspireServiceDiscoveryProviderFactory(IOcelotLoggerFactory factory, IServiceProvider provider)
        {
            _factory = factory;
            _provider = provider;
        }

        public Response<IServiceDiscoveryProvider> Get(ServiceProviderConfiguration serviceConfig, DownstreamRoute route)
        {
            var configuration = _provider.GetRequiredService<IConfiguration>();
            var informingConnectionString = configuration.GetConnectionString("informing");
            var identityConnectionString = configuration.GetConnectionString("identity");
            var x = configuration.GetValue("services:identity:http:0", "");
            var y = new Uri("https+http://informing");
            List<string> strings = new List<string>();
            configuration.Bind("services", strings);
            var xx = configuration.GetSection("services");
            var xxx = xx.GetChildren();
            configuration.Bind("services:identity:http", strings);
            var serviceName = route.ServiceName;
            var provider = new AspireServiceDiscoveryProvider(_provider, serviceConfig, route);
            return new OkResponse<IServiceDiscoveryProvider>(provider);
        }

        //public async Task<List<Service>> GetAsync()
        //{
        //    var x = _downstreamRoute.UpstreamPathTemplate;
        //    return new List<Service>
        //    {
        //        new Service("informing", new ServiceHostAndPort("", 80, "http"), "id", "1", new string[]{ "latest" })
        //    };
        //}
    }
}
