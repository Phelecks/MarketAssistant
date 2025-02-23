using BaseApplication.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace WalletTracker.Api.Filters
{
    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails? validationProblemDetails)
        {
            if (validationProblemDetails is null) throw new Exception();

            return new BadRequestObjectResult(new BaseResponseDto(ResponseCode.BadRequest, System.Text.Json.JsonSerializer.Serialize(validationProblemDetails.Errors)));
        }
    }
}
