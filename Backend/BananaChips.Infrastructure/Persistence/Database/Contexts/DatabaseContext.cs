using BananaChips.Domain.Entities;
using BananaChips.Infrastructure.Persistence.Database.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BananaChips.Infrastructure.Persistence.Database.Contexts;

public class DatabaseContext : IdentityDbContext<User>
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceLine> InvoiceLines { get; set; }
    public DbSet<Company> Companies { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>()
            .Property(u => u.FullName).HasComputedColumnSql("CONCAT([FirstName], ' ', [LastName])", stored: true);
        modelBuilder.Entity<Company>()
            .HasIndex(c => c.Identifier)
            .IsUnique();
        modelBuilder.Entity<Invoice>()
            .Property(i => i.NetValue)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Invoice>()
            .Property(i => i.GrossValue)
            .HasPrecision(19, 4);
        modelBuilder.Entity<InvoiceLine>()
            .Property(il => il.NetValue)
            .HasPrecision(19, 4);
        modelBuilder.Entity<InvoiceLine>()
            .Property(il => il.GrossValue)
            .HasPrecision(19, 4);
        modelBuilder.Entity<InvoiceLine>()
            .Property(il => il.UnitPrice)
            .HasPrecision(19, 4);
        modelBuilder.Entity<Company>()
            .HasOne(c => c.Address)
            .WithOne(a => a.Company)
            .HasForeignKey<Address>(a => a.CompanyId);
        modelBuilder.AddSoftDeleteQueryFilter();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        this.SetModifiedEntitiesTimestamp();
        return base.SaveChangesAsync(cancellationToken);
    }
}