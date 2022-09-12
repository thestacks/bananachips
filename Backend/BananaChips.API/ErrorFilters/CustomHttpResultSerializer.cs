using System.Net;
using HotChocolate.AspNetCore.Serialization;
using HotChocolate.Execution;

namespace BananaChips.API.ErrorFilters;

public class CustomHttpResultSerializer : DefaultHttpResultSerializer
{
    private const string AuthorizationErrorCode = "AUTH_NOT_AUTHENTICATED";
    public override HttpStatusCode GetStatusCode(IExecutionResult result)
    {
        var statusCode = base.GetStatusCode(result);
        if (statusCode != HttpStatusCode.InternalServerError) return statusCode;
        
        if (result.Errors.Any(e => e.Extensions.ContainsKey(CustomExceptionErrorFilter.ExtensionControlledErrorTypeKey) &&
                                   e.Extensions[CustomExceptionErrorFilter.ExtensionControlledErrorTypeKey] == CustomExceptionErrorFilter.ExtensionsControlledErrorTypeValidation))
            statusCode = HttpStatusCode.BadRequest;
        else if (result.Errors.Any(e => e.Code == AuthorizationErrorCode))
            statusCode = HttpStatusCode.Unauthorized;

        return statusCode;
    }
}