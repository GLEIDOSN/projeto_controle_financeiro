
using Blazor_Finance_Web.Data;
using Blazor_Finance_Web.Services.Interfaces;
using Blazor_Finance_Web.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Blazor_Finance_Web.Models;
using Blazor_Finance_Web.Enums;

var builder = WebApplication.CreateBuilder(args);

// Configure a licen�a do QuestPDF
QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddHttpClient();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("FinanceWebContext") ?? throw new InvalidOperationException("Connection string 'FinanceWebContext' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IContaService, ContaService>();
builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorizationCore();

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

// Configura o seeding do usu�rio padr�o
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var usuarioService = scope.ServiceProvider.GetRequiredService<IUsuarioService>();

    // Aplica as migra��es para garantir que o banco est� atualizado
    await dbContext.Database.MigrateAsync();

    // Verifica se h� algum usu�rio no banco; se n�o, cria um usu�rio padr�o
    if (!await dbContext.Usuarios.AnyAsync())
    {
        var usuarioPadrao = new Usuario
        {
            Nome = "Admin",
            TipoUsuario = TipoUsuario.Admin,
            Email = "admin@hotmail.com",
            Senha = "admin"
        };

        // Adiciona o usu�rio usando o servi�o
        await usuarioService.AddUsuarioAsync(usuarioPadrao);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
