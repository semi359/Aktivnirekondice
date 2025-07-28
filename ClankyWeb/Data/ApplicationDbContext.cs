using ClankyWeb.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClankyWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<NUser, NRole, int>
    {
        public DbSet<Clanek> Clanky => Set<Clanek>();
        public DbSet<Komentar> Komentare => Set<Komentar>();

        public DbSet<ReservationEvent> ReservationEvents => Set<ReservationEvent>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Komentar>()
                .HasOne(x => x.Clanek)
                .WithMany(x => x.Komentare)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Clanek>()
                .HasMany(x => x.Komentare)
                .WithOne(x => x.Clanek)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

            builder.Entity<NRole>().HasData(
                new NRole { Id = 1, Name = NRole.Admin, NormalizedName = NRole.Admin.ToUpper() },
                new NRole { Id = 2, Name = NRole.Autor, NormalizedName = NRole.Autor.ToUpper() },
                new NRole { Id = 3, Name = NRole.Ctenar, NormalizedName = NRole.Ctenar.ToUpper() }
            );
        }

    }

}
