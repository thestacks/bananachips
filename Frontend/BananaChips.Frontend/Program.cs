using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BananaChips.Frontend;
using BananaChips.Frontend.Authentication;
using BananaChips.Frontend.Services;
using BananaChips.Frontend.Services.Meta;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IBackendConnector, CompanyClient>();
builder.Services.AddBlazoredLocalStorage(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services
    .AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>()
    .AddScoped<IAuthenticationService, JwtAuthenticationService>()
    .AddScoped<ISessionManager, LocalStorageSessionManager>()
    .AddScoped<INotificationService, SnackbarNotificationService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
await builder.Build().RunAsync();