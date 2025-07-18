using MediatR.Interfaces;
using BaseDomain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BaseInfrastructure.Persistence.Interceptors;

public class DispatchDomainNotificationsInterceptor(IRequestDispatcher dispatcher) : SaveChangesInterceptor
{
    private readonly IRequestDispatcher _dispatcher = dispatcher;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var token = new CancellationTokenSource().Token;
        DispatchDomainNotifications(eventData.Context, token).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);

    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainNotifications(eventData.Context, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainNotifications(DbContext? context, CancellationToken cancellationToken)
    {
        if (context == null) return;

        if (context.ChangeTracker.Entries<BaseEntity>().Any())
        {
            var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainNotifications.Count != 0)
            .Select(e => e.Entity);

            var DomainNotifications = entities
                .SelectMany(e => e.DomainNotifications)
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainNotifications());

            foreach (var domainEvent in DomainNotifications)
                await _dispatcher.PublishAsync(domainEvent, cancellationToken);
        }
        if (context.ChangeTracker.Entries<BaseEntityWithNoPrimaryKey>().Any())
        {
            var entities = context.ChangeTracker
            .Entries<BaseEntityWithNoPrimaryKey>()
            .Where(e => e.Entity.DomainNotifications.Count != 0)
            .Select(e => e.Entity);

            var DomainNotifications = entities
                .SelectMany(e => e.DomainNotifications)
                .ToList();

            entities.ToList().ForEach(e => e.ClearDomainNotifications());

            foreach (var domainEvent in DomainNotifications)
                await _dispatcher.PublishAsync(domainEvent, cancellationToken);
        }
    }
}
