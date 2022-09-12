using BananaChips.Domain.Entities;
using BananaChips.Infrastructure.Persistence.Database.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Application.Actions.Invoices.Queries;

public class GetInvoices
{
    public record Query : IRequest<IQueryable<Invoice>>;
    public class Handler : IRequestHandler<Query, IQueryable<Invoice>>
    {
        private readonly DatabaseContext _context;
        
        public Handler(IDbContextFactory<DatabaseContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }

        public Task<IQueryable<Invoice>> Handle(Query request, CancellationToken cancellationToken) =>
            Task.FromResult(_context.Invoices.AsNoTracking().AsQueryable());
    }
}