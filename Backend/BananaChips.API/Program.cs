using System.Reflection;
using BananaChips.API.ErrorFilters;
using BananaChips.API.Extensions;
using BananaChips.API.Mutations;
using BananaChips.API.Queries;
using BananaChips.Application;
using BananaChips.Infrastructure;
using HotChocolate.AspNetCore.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(Assembly.GetCallingAssembly());
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddInitializers()
    .AddIdentity(builder.Configuration, builder.Environment);
builder.Services
    .AddSingleton<IHttpResultSerializer, CustomHttpResultSerializer>()
    .AddGraphQLServer()
    .AddQueryType()
    .AddMutationType()
    .AddTypeExtension<CompanyQueries>()
    .AddTypeExtension<UserMutations>()
    .AddTypeExtension<InvoiceQueries>()
    .AddTypeExtension<CompanyMutations>()
    .AddTypeExtension<InvoiceMutations>()
    .AddFiltering()
    .AddProjections()
    .AddSorting()
    .AddAuthorization()
    .AddErrorFilter<CustomExceptionErrorFilter>();
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(p => 
        p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

var app = builder.Build();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication().UseAuthorization();
app.MapGraphQL();
app.Run();