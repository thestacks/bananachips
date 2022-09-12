using BananaChips.Application.Models.Session;
using BananaChips.Domain.Entities;
using BananaChips.Infrastructure.Services.Meta;
using Common.Validation;
using Common.Validation.Extensions;
using FluentValidation;
using HotChocolate;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BananaChips.Application.Actions.Session.Commands;

public class Login
{
    [GraphQLName("LoginInput")]
    public record Command(string Email, string Password) : IRequest<TokenResponse>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            
            RuleFor(r => r.Email).EmailAddress().WithErrorCode(ValidationErrorCode.INVALID_EMAIL);
            RuleFor(r => r.Password).NotEmpty().WithErrorCode(ValidationErrorCode.MISSING_REQUIRED_FIELDS);
        }
    }

    public class CommandHandler : IRequestHandler<Command, TokenResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public CommandHandler(UserManager<User> userManager, IAccessTokenProvider accessTokenProvider)
        {
            _userManager = userManager;
            _accessTokenProvider = accessTokenProvider;
        }

        public async Task<TokenResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                throw FluentValidationExtensions.CreateValidationException(ValidationErrorCode.INVALID_CREDENTIALS);

            var accessToken = _accessTokenProvider.GenerateAccessToken(user);
            var expiresIn = (int)Math.Ceiling((accessToken.expirationDate - DateTime.UtcNow).TotalSeconds);
            return new TokenResponse(accessToken.token, expiresIn);
        }
    }
}