using Grpc.Core;

namespace BaseApplication.Helpers;

public static class ResponseHelper
{
    private const string DefaultErrorMessage = "An error occurred during the process.";

    public static BaseResponseDto Success()
    {
        return new(
        ResponseCode.Success, "Success");
    }

    public static BaseResponseDto<T> Success<T>(T data)
    {
        return new(
            data: data, ResponseCode.Success, "Success");
    }

    public static BaseResponseDto Success(string responseInformation)
    {
        return new(
            ResponseCode.Success, responseInformation);
    }

    public static BaseResponseDto<T> Success<T>(T data, string responseInformation)
    {
        return new(
            data, ResponseCode.Success, responseInformation);
    }

    public static BaseResponseDto Error()
    {
        return new(
        ResponseCode.Fail, DefaultErrorMessage);
    }

    public static BaseResponseDto<T> Error<T>()
    {
        return new(
            default!, ResponseCode.Fail, DefaultErrorMessage);
    }

    public static BaseResponseDto Error(ResponseCode responseCode)
    {
        return new(
            responseCode, DefaultErrorMessage);
    }

    public static BaseResponseDto<T> Error<T>(ResponseCode responseCode)
    {
        return new(
            default!, responseCode, DefaultErrorMessage);
    }

    public static BaseResponseDto Error(string responseInformation)
    {
        return new(
            ResponseCode.Fail, string.IsNullOrEmpty(responseInformation) ? DefaultErrorMessage : responseInformation);
    }

    public static BaseResponseDto<T> Error<T>(string responseInformation)
    {
        return new(
            default!, ResponseCode.Fail, string.IsNullOrEmpty(responseInformation) ? DefaultErrorMessage : responseInformation);
    }

    public static BaseResponseDto Error(ResponseCode responseCode, string responseInformation)
    {
        return new(
            responseCode, responseInformation);
    }

    public static BaseResponseDto<T> Error<T>(ResponseCode responseCode, string responseInformation)
    {
        return new(
            default!, responseCode, responseInformation);
    }

    public static BaseResponseDto NotFound()
    {
        return new(
        ResponseCode.NotFound, "Record cannot be found.");
    }

    public static BaseResponseDto<T> NotFound<T>()
    {
        return new(
            default!, ResponseCode.NotFound, "Record cannot be found.");
    }

    public static BaseResponseDto NotFound(string responseInformation)
    {
        return new(
            ResponseCode.NotFound, responseInformation);
    }

    public static BaseResponseDto<T> NotFound<T>(string responseInformation)
    {
        return new(
            default!, ResponseCode.NotFound, responseInformation);
    }

    public static BaseResponseDto BadRequest()
    {
        return new(
        ResponseCode.BadRequest, "Your input does not meet the required values.");
    }

    public static BaseResponseDto<T> BadRequest<T>()
    {
        return new(
            default!, ResponseCode.BadRequest, "Your input does not meet the required values.");
    }

    public static BaseResponseDto BadRequest(string responseInformation)
    {
        return new(
            ResponseCode.BadRequest, responseInformation);
    }

    public static BaseResponseDto<T> BadRequest<T>(string responseInformation)
    {
        return new(
            default!, ResponseCode.BadRequest, responseInformation);
    }

    public static BaseResponseDto Forbidden()
    {
        return new(
        ResponseCode.Unauthorized, "Access denied.");
    }

    public static BaseResponseDto<T> Forbidden<T>()
    {
        return new(
            default!, ResponseCode.Unauthorized, "Access denied.");
    }

    public static BaseResponseDto Forbidden(string responseInformation)
    {
        return new(
            ResponseCode.Unauthorized, responseInformation);
    }

    public static BaseResponseDto<T> Forbidden<T>(string responseInformation)
    {
        return new(
            default!, ResponseCode.Unauthorized, responseInformation);
    }
}

public class BaseResponseDto
{
    public BaseResponseDto(ResponseCode responseCode, string responseInformation)
    {
        this.responseCode = responseCode;
        this.responseInformation = responseInformation;
    }

    private ResponseCode responseCode { get; }
    private string responseInformation { get; }

    private bool isSuccess => responseCode == ResponseCode.Success;

    public bool IsSuccess()
    {
        return isSuccess;
    }

    public string ResponseInformation => responseInformation;

    public StatusCode GrpcStatusCode()
    {
        return responseCode switch
        {
            ResponseCode.Success => StatusCode.OK,
            ResponseCode.NotFound => StatusCode.NotFound,
            ResponseCode.BadRequest => StatusCode.InvalidArgument,
            ResponseCode.Unauthorized => StatusCode.Unauthenticated,
            _ => StatusCode.Unknown
        };
    }

    public ResponseCode ResponseCode => responseCode;
}

public class BaseResponseDto<T> : BaseResponseDto
{
    /// <summary>
    /// Response data
    /// </summary>
    public T data { get; }

    public BaseResponseDto(T data, ResponseCode responseCode, string responseInformation) : base(responseCode, responseInformation)
    {
        this.data = data;
    }
}

/// <summary>
/// Response Code
/// </summary>
public enum ResponseCode
{
    /// <summary>
    /// Action executed with success response
    /// </summary>
    Success = 0,
    /// <summary>
    /// Multi status, it contains success, not found  and other codes
    /// </summary>
    MultiStatus = 207,
    /// <summary>
    /// Not found
    /// </summary>
    NotFound = 404,
    /// <summary>
    /// Bad request
    /// </summary>
    BadRequest = 400,
    /// <summary>
    /// Unauthorized
    /// </summary>
    Unauthorized = 401,
    /// <summary>
    /// Failed to execute action
    /// </summary>
    Fail = 99
}