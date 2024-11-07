using FinanceWeb.Data;
using FinanceWeb.Services;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure a licença do QuestPDF
        QuestPDF.Settings.License = LicenseType.Community;

        builder.Services.AddDbContextFactory<FinanceWebContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("FinanceWebContext") ?? throw new InvalidOperationException("Connection string 'FinanceWebContext' not found.")));

        builder.Services.AddControllers();
        builder.Services.AddScoped<ReportService>();

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddQuickGridEntityFrameworkAdapter();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();
        app.MapControllers();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}