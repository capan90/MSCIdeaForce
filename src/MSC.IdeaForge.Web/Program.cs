using Microsoft.EntityFrameworkCore;
using MSC.IdeaForge.Application;
using MSC.IdeaForge.Infrastructure;
using MSC.IdeaForge.Infrastructure.Persistence;

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

// Veritabanını otomatik migrate et
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();

