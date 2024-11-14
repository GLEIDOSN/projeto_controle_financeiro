using Blazor_Finance_Web.Models;
using Blazor_Finance_Web.Services.Interfaces;
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
    }    
}
