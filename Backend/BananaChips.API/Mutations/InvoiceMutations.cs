using BananaChips.Application.Actions.Invoices.Commands;
using BananaChips.Application.Models.Common;
using BananaChips.Domain.Entities;
using HotChocolate.AspNetCore.Authorization;
using MediatR;

namespace BananaChips.API.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class InvoiceMutations
{
    [Authorize]
    public Task<Invoice> CreateInvoice(CreateInvoice.Command input, CancellationToken cancellationToken,
        [Service] IMediator mediator) => mediator.Send(input, cancellationToken);

    [Authorize]
    public Task<BasicResponse> DeleteInvoice(DeleteInvoice.Command input, CancellationToken cancellationToken,
        [Service] IMediator mediator) => mediator.Send(input, cancellationToken);
}