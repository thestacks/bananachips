using FluentValidation;

namespace BananaChips.API.ErrorFilters;

public class CustomExceptionErrorFilter : IErrorFilter
{
    private const string Message =
        "An error occured and has been handled by the server. Please refer to the code extensions.code property.";
    public const string ExtensionControlledErrorTypeKey = "type";
    public const string ExtensionsControlledErrorTypeValidation = "validation";
    
    public IError OnError(IError error)
    {
        if (error.Exception is ValidationException validationException)
        {
            return new Error(Message,
                validationException.Errors.FirstOrDefault(e => !string.IsNullOrEmpty(e.ErrorCode))?.ErrorCode).SetExtension(ExtensionControlledErrorTypeKey, ExtensionsControlledErrorTypeValidation);
        }

        return error;
    }
}