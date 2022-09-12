using BananaChips.Application.Actions.Session.Commands;
using BananaChips.Application.Models.Session;
using MediatR;

namespace BananaChips.API.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class UserMutations
{
    public Task<TokenResponse> LoginAsync(Login.Command input, CancellationToken cancellationToken,
        [Service] IMediator mediator) => mediator.Send(input, cancellationToken);
}