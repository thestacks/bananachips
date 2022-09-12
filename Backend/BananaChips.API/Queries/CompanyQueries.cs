using BananaChips.Application.Actions.Company.Queries;
using BananaChips.Domain.Entities;
using HotChocolate.AspNetCore.Authorization;
using MediatR;

namespace BananaChips.API.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class CompanyQueries
{
    [Authorize]
    [UseOffsetPaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<Company>> GetCompanies([Service] IMediator mediator, CancellationToken cancellationToken) =>
        mediator.Send(new GetCompanies.Query(), cancellationToken);
}