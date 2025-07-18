using System.Reflection;
using BaseApplication.Exceptions;
using BaseApplication.Security;
using IdentityHelper.Helpers;
using MediatR.Attributes;
using MediatR.Interfaces;

namespace BaseApplication.Behaviors;

[BehaviorOrder(2)]
public class AuthorizationBehavior<TRequest, TResponse>(IIdentityHelper identityHelper) 
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IIdentityHelper _identityHelper = identityHelper;

    public async Task<TResponse> HandleAsync(TRequest request, Func<CancellationToken, Task<TResponse>> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();
        if (authorizeAttributes.Any())
        {
            // Must be authenticated user
            if (string.IsNullOrEmpty(_identityHelper.GetUserIdentity())) throw new UnauthorizedAccessException();
            // Role-based authorization
            var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.roles));
            if (authorizeAttributesWithRoles.Any())
            {
                var authorized = authorizeAttributesWithRoles.Select(a => a.roles.Split(',')).Any(roles => roles.Any(role => _identityHelper.IsInRole(role)));
                // Must be a member of at least one role in roles
                if (!authorized) throw new ForbiddenAccessException("Access denied.");
            }
            // Policy-based authorization
            var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.policy));
            if (authorizeAttributesWithPolicies.Any())
            {
                var authorized = authorizeAttributesWithPolicies.Select(a => a.policy).Any(policy => _identityHelper.IsAuthorized(policy));
                if (!authorized) throw new ForbiddenAccessException("Access denied.");
            }
        }
        return await next(cancellationToken);
    }
}
