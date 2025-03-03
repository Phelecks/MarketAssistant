using BaseApplication.Interfaces;
using BaseDomain.Common;
using IdentityHelper.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BaseInfrastructure.Persistence.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IIdentityHelper _identityHelper;
    private readonly IDateTimeService _dateTimeService;

    public AuditableEntityInterceptor(
        IIdentityHelper identityHelper,
        IDateTimeService dateTimeService)
    {
        _identityHelper = identityHelper;
        _dateTimeService = dateTimeService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        if(context.ChangeTracker.Entries<BaseAuditEntity>().Any())
            foreach (var entry in context.ChangeTracker.Entries<BaseAuditEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _identityHelper.GetUserIdentity() ?? "System";
                    entry.Entity.Created = _dateTimeService.UtcNow;
                }

                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModifiedBy = _identityHelper.GetUserIdentity() ?? "System";
                    entry.Entity.LastModified = _dateTimeService.UtcNow;
                }
            }
        if(context.ChangeTracker.Entries<BaseAuditEntityWithNoPrimaryKey>().Any())
            foreach (var entry in context.ChangeTracker.Entries<BaseAuditEntityWithNoPrimaryKey>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _identityHelper.GetUserIdentity() ?? "System";
                    entry.Entity.Created = _dateTimeService.UtcNow;
                }

                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    entry.Entity.LastModifiedBy = _identityHelper.GetUserIdentity() ?? "System";
                    entry.Entity.LastModified = _dateTimeService.UtcNow;
                }
            }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
