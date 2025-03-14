using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using BaseApplication.Helpers;
using BaseApplication.Exceptions;
using LoggerService.Helpers;

namespace BaseInfrastructure.Interceptors;

public class GrpcGlobalExceptionHandlerInterceptor(ILogger<GrpcGlobalExceptionHandlerInterceptor> logger) : Interceptor
{
    private readonly ILogger<GrpcGlobalExceptionHandlerInterceptor> _logger = logger;

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }
        catch (NotFoundException exception)
        {
            var response = ResponseHelper.NotFound(exception.Message);
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
        }
        catch (ValidationException exception)
        {
            var response = ResponseHelper.BadRequest(exception.Message);
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
        }
        catch (ForbiddenAccessException exception)
        {
            var response = ResponseHelper.Forbidden(exception.Message);
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
        }
        catch (UnauthorizedAccessException exception)
        {
            var response = ResponseHelper.Forbidden(exception.Message);
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.General, eventName: "Exception"),
                exception, nameof(GrpcGlobalExceptionHandlerInterceptor)), context.CancellationToken);

            var response = ResponseHelper.Error();
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
        }
    }
}