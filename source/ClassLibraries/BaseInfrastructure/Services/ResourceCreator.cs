using MassTransitManager.Helpers;
using MassTransitManager.Messages;
using MassTransitManager.Messages.Interfaces;
using MassTransitManager.Services;

namespace BaseInfrastructure.Services
{
    public static class ResourceCreator
    {
        public static async Task CreateResourceAsyc(IMassTransitService massTransitService, string applicationName)
        {
            await massTransitService.SendAsync<ICreateResourceMessage>(new CreateResourceMessage(applicationName), Queues.CreateResourceMessageQueueName);
        }
    }
}
