using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class Contexto : IdentityDbContext
{
    public DbSet<Ocupaciones> Ocupaciones { get; set; }
    public DbSet<Personas> Personas { get; set; }
    public DbSet<Prestamos> Prestamos { get; set; }
    public DbSet<Pagos> Pagos { get; set; }
    public Contexto(DbContextOptions<Contexto> options)
        : base(options)
    {
    }
}
