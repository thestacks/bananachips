using BananaChips.Application.Actions.Invoices.Queries;
using BananaChips.Domain.Entities;
using HotChocolate.AspNetCore.Authorization;
using MediatR;

namespace BananaChips.API.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class InvoiceQueries
{
    [Authorize]
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<Invoice>> GetInvoices([Service] IMediator mediator, CancellationToken cancellationToken) =>
        mediator.Send(new GetInvoices.Query(), cancellationToken);
}