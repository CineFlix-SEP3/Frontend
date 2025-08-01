using Frontend.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProtectedSessionStorage>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<Frontend.Services.CustomAuthService>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7086");
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new CookieContainer()
    });

builder.Services.AddHttpClient<Frontend.Services.UserClient>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7086");
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new CookieContainer()
    });

builder.Services.AddHttpClient<Frontend.Services.MovieClient>(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7086");
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new CookieContainer()
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();