using BananaChips.Application.Models.Common;
using BananaChips.Infrastructure.Persistence.Database.Contexts;
using Common.Validation;
using Common.Validation.Extensions;
using FluentValidation;
using HotChocolate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Application.Actions.Invoices.Commands;

public class DeleteInvoice
{
    [GraphQLName("DeleteInvoiceInput")]
    public record Command(long Id) : IRequest<BasicResponse>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithErrorCode(ValidationErrorCode.MISSING_REQUIRED_FIELDS);
        }
    }

    public class Handler : IRequestHandler<Command, BasicResponse>
    {
        private readonly DatabaseContext _context;
        
        public Handler(IDbContextFactory<DatabaseContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }
        
        public async Task<BasicResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.Include(i => i.Lines).SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken) ??
                          throw FluentValidationExtensions.CreateValidationException(ValidationErrorCode.INVOICE_NOT_FOUND);

            foreach (var invoiceLine in invoice.Lines)
                invoiceLine.Deleted = true;

            invoice.Deleted = true;
            await _context.SaveChangesAsync();
            
            return BasicResponse.Successful;
        }
    }
}