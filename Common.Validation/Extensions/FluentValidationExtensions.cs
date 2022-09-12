using FluentValidation;
using FluentValidation.Results;

namespace Common.Validation.Extensions;
public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithErrorCode<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, ValidationErrorCode validationErrorCode)
        => rule.WithErrorCode(validationErrorCode.ToString());

    public static ValidationException CreateValidationException(ValidationErrorCode validationErrorCode) =>
        new ValidationException(new List<ValidationFailure>
            { new ValidationFailure { ErrorCode = validationErrorCode.ToString() } });
}