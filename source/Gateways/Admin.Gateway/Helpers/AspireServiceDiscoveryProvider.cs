using Ocelot.Configuration;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;

namespace Admin.Gateway.Helpers
{
    public class AspireServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private readonly DownstreamRoute _downstreamRoute;
        private readonly IConfiguration _configuration;

        public AspireServiceDiscoveryProvider(DownstreamRoute downstreamRoute, IConfiguration configuration)
        {
            _downstreamRoute = downstreamRoute;
            _configuration = configuration;
        }

        public async Task<List<Service>> GetAsync()
        {
            var x = _downstreamRoute.UpstreamPathTemplate;
            return new List<Service>
            {
                new Service("informing", new ServiceHostAndPort("", 80, "http"), "id", "1", new string[]{ "latest" })
            };
        }
    }
}
