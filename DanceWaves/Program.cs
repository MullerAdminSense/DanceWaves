using Microsoft.AspNetCore.Components.Authorization;
using DanceWaves.Infrastructure.Security;
using DanceWaves.Client.Pages;
using DanceWaves.Components;
using Microsoft.EntityFrameworkCore;
using DanceWaves.Data;
using DanceWaves.Application.Ports;
using DanceWaves.Application.UseCases;
using DanceWaves.Adapters.Persistence;
using DanceWaves.Adapters.Presenters;
using Microsoft.AspNetCore.Identity;
using DanceWaves.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Configure EF DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Federated Authentication (Microsoft Entra ID B2C)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies")
    .AddOpenIdConnect("oidc", options =>
    {
        var config = builder.Configuration.GetSection("AzureAdB2C");
        options.Authority = $"{config["Instance"]}/tfp/{config["TenantId"]}/{config["SignUpSignInPolicyId"]}/v2.0/";
        options.ClientId = config["ClientId"];
        options.ClientSecret = config["ClientSecret"];
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.CallbackPath = config["CallbackPath"] ?? "/signin-oidc";
        options.SignedOutCallbackPath = config["SignedOutCallbackPath"] ?? "/signout-oidc";
    });

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticatedHttpClientHandler>();
builder.Services.AddHttpClient("SecureApiClient")
    .AddHttpMessageHandler<AuthenticatedHttpClientHandler>();

// Register Hexagonal Architecture - Ports
builder.Services.AddScoped<IEntryPersistencePort, EntryPersistenceAdapter>();
builder.Services.AddScoped<IUserPersistencePort, UserPersistenceAdapter>();
builder.Services.AddScoped<INavigationPresenterPort, NavigationPresenterAdapter>();
builder.Services.AddScoped<IAuthenticationPort, AuthenticationAdapter>();

// Register Use Cases
builder.Services.AddScoped<GetNavigationMenuUseCase>();
builder.Services.AddScoped<ListEntriesUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<FederatedLoginUseCase>();
builder.Services.AddScoped<GetCurrentUserUseCase>();
builder.Services.AddScoped<UpdateProfileUseCase>();
builder.Services.AddScoped<ChangePasswordUseCase>();

var app = builder.Build();

// Initialize database with seed data
await app.InitializeDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DanceWaves.Client._Imports).Assembly);

app.Run();
