using ApartmentManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Apartment> Apartments => Set<Apartment>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            // User (1) — (N) Apartment
            b.Entity<Apartment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Apartments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict); // İstersen Cascade yapabilirsin

            // Sık arama için index (opsiyonel ama faydalı)
            b.Entity<Apartment>().HasIndex(a => a.Title);
        }
    }
}
