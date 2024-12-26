using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ErkekKuaforu_WebProgramlama.Veritabani
{
    public class VeritabaniContext : IdentityDbContext<Kisi, Rol, string>
    {
        public VeritabaniContext(DbContextOptions<VeritabaniContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Calisan>()
                .HasOne(c => c.Kisi) 
                .WithMany(k => k.Calisans) 
                .HasForeignKey(c => c.KisiId) 
                .OnDelete(DeleteBehavior.Restrict); 

            
            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.Kisi) 
                .WithMany(k => k.CalisanHizmets) 
                .HasForeignKey(ch => ch.KisiId) 
                .OnDelete(DeleteBehavior.Restrict); 

           
            modelBuilder.Entity<CalisanHizmet>()
                .HasOne(ch => ch.Hizmet) 
                .WithMany(h => h.CalisanHizmets) 
                .HasForeignKey(ch => ch.HizmetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(c => c.Musteri)
                .WithMany()
                .HasForeignKey(c => c.MusteriId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        DbSet<Calisan> CalisanBilgileri { get; set; }
        DbSet<CalisanHizmet> CalisanHizmetleri { get; set; }
        DbSet<Hizmet> Hizmetler { get; set; }
        DbSet<Randevu> Randevular { get; set; }
    }
}
