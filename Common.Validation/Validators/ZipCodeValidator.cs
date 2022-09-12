using FluentValidation;
using Common.Validation.Extensions;

namespace Common.Validation.Validators;

public static class ZipCodeValidator
{
    public static IRuleBuilderOptions<T, string> ZipCode<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().Matches(@"^[0-9]{2}-[0-9]{3}")
            .WithErrorCode(ValidationErrorCode.INVALID_POSTAL_CODE);
    }
}