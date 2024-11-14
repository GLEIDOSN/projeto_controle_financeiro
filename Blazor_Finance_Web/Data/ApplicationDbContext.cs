using Blazor_Finance_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Finance_Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = default!;
        public DbSet<Conta> Contas { get; set; } = default!;

        public static async Task SeedData()
        {
            await Task.Delay(100);
            
        }
    }    
}
