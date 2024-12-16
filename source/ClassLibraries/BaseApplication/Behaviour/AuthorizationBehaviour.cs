using System.Reflection;
using BaseApplication.Exceptions;
using BaseApplication.Security;
using IdentityHelper.Helpers;
using MediatR;

namespace BaseApplication.Behaviour;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IIdentityHelper _identityHelper;
    //private readonly IIdentityService _identityService;

    public AuthorizationBehaviour(
        IIdentityHelper identityHelper/*, IIdentityService identityService*/)
    {
        _identityHelper = identityHelper;
        //_identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            //try authenticate and authorize with SIWE

            // Must be authenticated user
            if (string.IsNullOrEmpty(_identityHelper.GetUserIdentity()))
            {
                throw new UnauthorizedAccessException();
            }

            // Role-based authorization
            var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.roles));

            if (authorizeAttributesWithRoles.Any())
            {
                var authorized = false;

                foreach (var roles in authorizeAttributesWithRoles.Select(a => a.roles.Split(',')))
                {
                    foreach (var role in roles)
                    {
                        //var isInRole = await _identityService.IsInRoleAsync(_identityHelper.GetUserIdentity(), role.Trim());
                        var isInRole = _identityHelper.IsInRole(role);
                        if (isInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                }

                // Must be a member of at least one role in roles
                if (!authorized)
                {
                    throw new ForbiddenAccessException("Access denied.");
                }
            }

            // Policy-based authorization
            var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.policy));
            if (authorizeAttributesWithPolicies.Any())
            {
                foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.policy))
                {
                    //var authorized = await _identityService.AuthorizeAsync(_identityHelper.GetUserIdentity(), policy);
                    var authorized = _identityHelper.IsAuthorized(policy);
                    if (!authorized)
                    {
                        throw new ForbiddenAccessException("Access denied.");
                    }
                }
            }
        }

        // User is authorized / authorization not required
        return await next();
    }
}
