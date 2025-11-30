using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BeFit.Models;

namespace BeFit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // POPRAWNE NAZWY DbSet!
        public DbSet<Cwiczenie> Cwiczenie { get; set; }
        public DbSet<TypCwiczenia> TypCwiczenia { get; set; }
        public DbSet<Sesja> Sesja { get; set; }
    }
}