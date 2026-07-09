using MSC.IdeaForge.Application;
using MSC.IdeaForge.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Yerel yapılandırma dosyasını (varsa) konfigürasyon pipeline'ına ekliyoruz
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<MSC.IdeaForge.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
