using Microsoft.EntityFrameworkCore;
using FinanceWeb.Models;

namespace FinanceWeb.Data
{
    public class FinanceWebContext : DbContext
    {
        public FinanceWebContext (DbContextOptions<FinanceWebContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = default!;
        public DbSet<Conta> Contas { get; set; } = default!;
    }
}
