using Grupa5Tim3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Grupa5Tim3.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Korisnik> Korisnik { get; set; }
    public DbSet<Aukcija> Aukcija { get; set; }
    public DbSet<KorisnikAukcija> KorisnikAukcija { get; set; }
    public DbSet<Notifikacija> Notifikacija { get; set; }
    public DbSet<NotifikacijaKorisnik> NotifikacijaKorisnik { get; set; }
    public DbSet<NotifikacijaUmjetnina> NotifikacijaUmjetnina {  get; set; }
    public DbSet<Umjetnina> Umjetnina { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Korisnik>().ToTable("Korisnik");
        modelBuilder.Entity<Aukcija>().ToTable("Aukcija");
        modelBuilder.Entity<KorisnikAukcija>().ToTable("KorisnikAukcija");
        modelBuilder.Entity<Notifikacija>().ToTable("Notifikacija");
        modelBuilder.Entity<NotifikacijaKorisnik>().ToTable("NotifikacijaKorisnik");
        modelBuilder.Entity<NotifikacijaUmjetnina>().ToTable("NotifikacijaUmjetnina");
        modelBuilder.Entity<Umjetnina>().ToTable("Umjetnina");

        base.OnModelCreating(modelBuilder);
    }
}
