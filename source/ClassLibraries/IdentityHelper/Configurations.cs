using IdentityHelper.Services;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Siwe;
using NethereumStorage;

namespace IdentityHelper
{
    public static class Configurations
    {
        public static void AddIdentityHelperDependencyInjections(this IServiceCollection services)
        {
            services.AddScoped<ISiweService, SiweService>();
            services.AddScoped<CustomSessionStorage>();
            services.AddScoped<ISessionStorage>(serviceProvider =>
            {
                return serviceProvider.GetRequiredService<CustomSessionStorage>();
            });
            services.AddScoped(serviceProvider =>
            {
                return new SiweMessageService(serviceProvider.GetRequiredService<CustomSessionStorage>(), null, null);
            });
        }
    }
}
