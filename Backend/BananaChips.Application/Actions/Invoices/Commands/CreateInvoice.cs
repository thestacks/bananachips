using BananaChips.Domain.Entities;
using BananaChips.Domain.Enums;
using BananaChips.Infrastructure.Extensions;
using BananaChips.Infrastructure.Persistence.Database.Contexts;
using Common.Validation;
using Common.Validation.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Application.Actions.Invoices.Commands;

public class CreateInvoice
{
    public record Command(string Name, long PublishDate, long PaymentDate, InvoiceType Type, IEnumerable<NewInvoiceLine> Lines,
        int SellerId, int BuyerId) : IRequest<Invoice>;

    public record NewInvoiceLine(string Name, double Amount, decimal UnitPrice, InvoiceLineUnitType UnitType,
        int TaxPercentage);

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(r => r.Name).NotEmpty().WithErrorCode(ValidationErrorCode.INVOICE_REQUIRES_NAME);
            RuleFor(r => r.PaymentDate).GreaterThan(r => r.PublishDate)
                .WithErrorCode(ValidationErrorCode.INVOICE_PAYMENT_DATE_MUST_BE_AFTER_PUBLISH_DATE);
            RuleFor(r => r.SellerId).NotEmpty().NotEqual(r => r.BuyerId)
                .WithErrorCode(ValidationErrorCode.INVOICE_SELLER_AND_BUYER_SHOULD_BE_DIFFERENT);
            RuleFor(r => r.BuyerId).NotEmpty().NotEqual(r => r.SellerId)
                .WithErrorCode(ValidationErrorCode.INVOICE_SELLER_AND_BUYER_SHOULD_BE_DIFFERENT);
            RuleForEach(r => r.Lines).ChildRules(lines =>
            {
                lines.RuleFor(l => l.Name).NotEmpty().WithErrorCode(ValidationErrorCode.INVOICE_LINE_REQUIRES_NAME);
                lines.RuleFor(l => l.TaxPercentage).GreaterThanOrEqualTo(0)
                    .WithErrorCode(ValidationErrorCode.INVOICE_LINE_TAX_PERCENTAGE_MUST_BE_POSITIVE_NUMBER);
                lines.RuleFor(l => l.Amount).GreaterThan(0)
                    .WithErrorCode(ValidationErrorCode.INVOICE_LINE_AMOUNT_MUST_BE_GREATER_THAN_ZERO);
                lines.RuleFor(l => l.UnitPrice).GreaterThanOrEqualTo(decimal.Zero)
                    .WithErrorCode(ValidationErrorCode.INVOICE_LINE_UNIT_PRICE_MUST_BE_POSITIVE_NUMBER);
            });
        }
    }

    public class Handler : IRequestHandler<Command, Invoice>
    {
        private readonly DatabaseContext _context;
        
        public Handler(IDbContextFactory<DatabaseContext> contextFactory)
        {
            _context = contextFactory.CreateDbContext();
        }
        
        public async Task<Invoice> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await _context.Companies.AnyAsync(c => c.Id == request.BuyerId, cancellationToken) ||
                !await _context.Companies.AnyAsync(c => c.Id == request.SellerId, cancellationToken))
                throw FluentValidationExtensions.CreateValidationException(ValidationErrorCode.COMPANY_NOT_FOUND);

            if (await _context.Invoices.AnyAsync(
                    i => i.Name == request.Name, cancellationToken))
                throw FluentValidationExtensions.CreateValidationException(ValidationErrorCode
                    .INVOICE_WITH_GIVEN_NAME_ALREADY_EXISTS);

            var lines = request.Lines.Select(ConvertLine).ToList();

            var invoice = new Invoice
            {
                Name = request.Name,
                PublishDate = request.PublishDate.TimeFromUnixTimeStampMilliseconds(),
                PaymentDate = request.PaymentDate.TimeFromUnixTimeStampMilliseconds(),
                Type = request.Type,
                NetValue = lines.Sum(l => l.NetValue),
                GrossValue = lines.Sum(l => l.GrossValue),
                SellerId = request.SellerId,
                BuyerId = request.BuyerId,
                Lines = lines
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }
        
        private InvoiceLine ConvertLine(NewInvoiceLine line)
        {
            var output = new InvoiceLine
            {
                Name = line.Name,
                Amount = line.Amount,
                UnitPrice = line.UnitPrice,
                UnitType = line.UnitType,
                NetValue = Math.Round((decimal)line.Amount * line.UnitPrice, 2),
                TaxPercentage = line.TaxPercentage
            };
            output.GrossValue = Math.Round(output.NetValue + output.NetValue * ((decimal)output.TaxPercentage / 100), 2);
            return output;
        }
    }

}