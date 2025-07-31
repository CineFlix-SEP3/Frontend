using Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<Frontend.Services.CustomAuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7086");
});
builder.Services.AddHttpClient<Frontend.Services.UserClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7086");
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