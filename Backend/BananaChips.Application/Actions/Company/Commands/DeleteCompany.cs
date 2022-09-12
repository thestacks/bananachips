using BananaChips.Application.Models.Common;
using Common.Validation.Extensions;
using BananaChips.Infrastructure.Persistence.Database.Contexts;
using Common.Validation;
using FluentValidation;
using HotChocolate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Application.Actions.Company.Commands;

public class DeleteCompany
{
    [GraphQLName("DeleteCompanyInput")]
    public record Command(int Id) : IRequest<BasicResponse>;

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
        
        public Handler(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _context = dbContextFactory.CreateDbContext();
        }
        
        public async Task<BasicResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
                          throw FluentValidationExtensions.CreateValidationException(ValidationErrorCode.COMPANY_NOT_FOUND);
            if (await _context.Invoices.AnyAsync(i => i.BuyerId == company.Id || i.SellerId == company.Id,
                    cancellationToken))
                throw FluentValidationExtensions.CreateValidationException(ValidationErrorCode.CANNOT_DELETE_COMPANY_WITH_INVOICES);


            company.Deleted = true;
            await _context.SaveChangesAsync();
            return BasicResponse.Successful;
        }
    }
}