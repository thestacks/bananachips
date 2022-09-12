using BananaChips.Application.Actions.Company.Commands;
using BananaChips.Application.Models.Common;
using BananaChips.Domain.Entities;
using HotChocolate.AspNetCore.Authorization;
using MediatR;

namespace BananaChips.API.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class CompanyMutations
{
    [Authorize]
    public Task<Company> UpsertCompany(UpsertCompany.Command input, CancellationToken cancellationToken,
        [Service] IMediator mediator) => mediator.Send(input, cancellationToken);

    [Authorize]
    public Task<BasicResponse> DeleteCompany(DeleteCompany.Command input, CancellationToken cancellationToken,
        [Service] IMediator mediator) => mediator.Send(input, cancellationToken);
}