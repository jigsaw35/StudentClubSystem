using Microsoft.EntityFrameworkCore;
using StudentClubSystem.Models;

namespace StudentClubSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tablolarımız
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Email benzersiz (Unique) olsun
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // İLİŞKİ AYARLARI (Fluent API)

            // 1. Kulüp silinirse, Etkinlikleri de silinsin (Cascade Delete)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Club)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.KulupId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Etkinlik silinirse, Kayıtlar (EventRegistrations) da silinsin
            modelBuilder.Entity<EventRegistration>()
                .HasOne(er => er.Event)
                .WithMany(e => e.EventRegistrations)
                .HasForeignKey(er => er.EtkinlikId)
                .OnDelete(DeleteBehavior.Cascade);

            // 3. Kullanıcı silinirse, kayıtları silinsin (veya Restrict yapabilirsin)
            modelBuilder.Entity<EventRegistration>()
                .HasOne(er => er.User)
                .WithMany(u => u.EventRegistrations)
                .HasForeignKey(er => er.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}