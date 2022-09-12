using BananaChips.Infrastructure.Persistence.Database.Contexts;
using HotChocolate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Application.Actions.Company.Queries;

public class GetCompanies
{
    [GraphQLName("CompaniesQuery")]
    public record Query : IRequest<IQueryable<Domain.Entities.Company>>;

    public class Handler : IRequestHandler<Query, IQueryable<Domain.Entities.Company>>
    {
        private readonly DatabaseContext _context;

        public Handler(IDbContextFactory<DatabaseContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }

        public Task<IQueryable<Domain.Entities.Company>> Handle(Query request, CancellationToken cancellationToken) =>
            Task.FromResult(_context.Companies.AsNoTracking().AsQueryable());
    }
}