using Common.Validation.Utilities;
using Common.Validation.Extensions;
using FluentValidation;

namespace Common.Validation.Validators;

public static class CompanyIdentifierValidator
{
    public static IRuleBuilderOptions<T, string> CompanyIdentifier<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().Must(NipChecker.Check)
            .WithErrorCode(ValidationErrorCode.INVALID_COMPANY_IDENTIFIER);
    }
}