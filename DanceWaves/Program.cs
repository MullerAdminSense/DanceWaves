using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Serilog;
using DanceWaves;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 

SerilogConfig.ConfigureLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

 
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

 
builder.Services.AddHttpContextAccessor();

 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


 
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

 
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
 
builder.Services.AddScoped<IEntryPersistencePort, EntryPersistenceAdapter>();
builder.Services.AddScoped<IUserPersistencePort, UserPersistenceAdapter>();
builder.Services.AddScoped<INavigationPresenterPort, NavigationPresenterAdapter>();
builder.Services.AddScoped<IAuthenticationPort, AuthenticationAdapter>();

 
builder.Services.AddScoped<GetNavigationMenuUseCase>();
builder.Services.AddScoped<ListEntriesUseCase>();
builder.Services.AddScoped<LoginUseCase>();
builder.Services.AddScoped<RegisterUseCase>();
builder.Services.AddScoped<FederatedLoginUseCase>();
builder.Services.AddScoped<GetCurrentUserUseCase>();
builder.Services.AddScoped<UpdateProfileUseCase>();
builder.Services.AddScoped<ChangePasswordUseCase>();

var app = builder.Build();
Log.Information("DanceWaves application starting up");

 
await app.InitializeDatabaseAsync();


 
var supportedCultures = new[] { "en", "nl", "fr", "de" };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    FallBackToParentCultures = false,
    FallBackToParentUICultures = false
};
localizationOptions.RequestCultureProviders.Clear();
localizationOptions.RequestCultureProviders.Add(new CookieRequestCultureProvider());
app.UseRequestLocalization(localizationOptions);
var currentCulture = localizationOptions.DefaultRequestCulture.Culture;
if (currentCulture != null)
{
    CultureInfo.DefaultThreadCurrentCulture = currentCulture;
    CultureInfo.DefaultThreadCurrentUICulture = currentCulture;
}

 
 
app.MapGet("/setculture/{culture}", (string culture, HttpContext context, [FromQuery] string? returnUrl) =>
{
    var requestCulture = new RequestCulture(culture);
    var cookieValue = CookieRequestCultureProvider.MakeCookieValue(requestCulture);

    var cookieOptions = new CookieOptions
    {
        Path = "/",
        Expires = DateTimeOffset.UtcNow.AddYears(1),
        IsEssential = true,
        SameSite = SameSiteMode.Lax
    };

    if (context.Request.IsHttps)
    {
        cookieOptions.Secure = true;
    }

    context.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, cookieValue, cookieOptions);

    var targetUrl = "/";
    if (!string.IsNullOrWhiteSpace(returnUrl) && Uri.TryCreate(returnUrl, UriKind.Relative, out _))
    {
        targetUrl = returnUrl;
    }

    return Results.Redirect(targetUrl);
});

 
 
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.Use(async (context, next) =>
    {
        if (!context.Request.Path.StartsWithSegments("/setculture"))
        {
            if (context.Request.Scheme == "http")
            {
                var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
                context.Response.Redirect(httpsUrl, permanent: false);
                return;
            }
        }
        await next();
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DanceWaves.Client._Imports).Assembly);

app.Run();
