using BananaChips.Infrastructure.Persistence.Database.Contexts;
using Common.Validation;
using Common.Validation.Extensions;
using Common.Validation.Validators;
using FluentValidation;
using HotChocolate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Application.Actions.Company.Commands;

public class UpsertCompany
{
    [GraphQLName("UpsertCompanyInput")]
    public record Command
        (int? Id, string Name, string Identifier, Address Address) : IRequest<Domain.Entities.Company>
    {
        [GraphQLIgnore] public bool IsEdit => Id.HasValue;
    }

    public record Address(string Line, string City, string ZipCode, string Country);
    
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Identifier).CompanyIdentifier();
            RuleFor(r => r.Address).NotNull();
            RuleFor(r => r.Address.Line).NotEmpty();
            RuleFor(r => r.Address.City).NotEmpty();
            RuleFor(r => r.Address.Country).NotEmpty();
            RuleFor(r => r.Address.ZipCode).ZipCode();
        }
    }

    public class Handler : IRequestHandler<Command, Domain.Entities.Company>
    {
        private readonly DatabaseContext _context;
        
        public Handler(IDbContextFactory<DatabaseContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }
        public async Task<Domain.Entities.Company> Handle(Command request, CancellationToken cancellationToken)
        {
            var company = request.IsEdit
                ? await _context.Companies.Include(c => c.Address)
                      .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
                  throw FluentValidationExtensions.CreateValidationException(ValidationErrorCode.COMPANY_NOT_FOUND)
                : new Domain.Entities.Company { Address = new() };

            company.Name = request.Name;
            company.Identifier = request.Identifier;
            company.Address.City = request.Address.City;
            company.Address.Country = request.Address.Country;
            company.Address.Line = request.Address.Line;
            company.Address.ZipCode = request.Address.ZipCode;

            if (!request.IsEdit)
                _context.Companies.Add(company);

            await _context.SaveChangesAsync();

            return company;
        }
    }
}