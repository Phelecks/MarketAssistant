using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using BaseApplication.Helpers;
using BaseApplication.Exceptions;
using LoggerService.Helpers;

namespace BaseInfrastructure.Interceptors;

public class GrpcGlobalExceptionHandlerInterceptor : Interceptor
{
    private readonly ILogger<GrpcGlobalExceptionHandlerInterceptor> _logger;

    public GrpcGlobalExceptionHandlerInterceptor(ILogger<GrpcGlobalExceptionHandlerInterceptor> logger)
    {
        _logger = logger;
    }

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
            //return MapResponse<TRequest, TResponse>(ResponseHelper.NotFound(exception.Message));
        }
        catch (ValidationException exception)
        {
            var response = ResponseHelper.BadRequest(exception.Message);
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
            //return MapResponse<TRequest, TResponse>(ResponseHelper.BadRequest(string.IsNullOrEmpty(exception.Message) ? $"{string.Join(',', exception.Errors)}" : exception.Message));
        }
        catch (ForbiddenAccessException exception)
        {
            var response = ResponseHelper.Forbidden(exception.Message);
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
            //return MapResponse<TRequest, TResponse>(ResponseHelper.Forbidden(string.IsNullOrEmpty(exception.Message) ? null : exception.Message));
        }
        catch (UnauthorizedAccessException exception)
        {
            var response = ResponseHelper.Forbidden(exception.Message);
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
            //return MapResponse<TRequest, TResponse>(ResponseHelper.Forbidden(string.IsNullOrEmpty(exception.Message) ? null : exception.Message));
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                eventId: EventTool.GetEventInformation(eventType: EventType.General, eventName: "Exception"),
                exception, exception.Message), context.CancellationToken);

            var response = ResponseHelper.Error();
            throw new RpcException(new Status(response.GrpcStatusCode(), response.ResponseInformation));
            //return MapResponse<TRequest, TResponse>(ResponseHelper.Error());
        }
    }

    //private TResponse MapResponse<TRequest, TResponse>(BaseResponseDto dto)
    //{
    //    var concreteResponse = Activator.CreateInstance<TResponse>();

    //    concreteResponse?.GetType().GetProperty(nameof(dto.GetResponseCode))?.SetValue(concreteResponse, dto.GetResponseCode());

    //    concreteResponse?.GetType().GetProperty(nameof(dto.GetResponseInformation))?.SetValue(concreteResponse, dto.GetResponseInformation());

    //    return concreteResponse;
    //}
}