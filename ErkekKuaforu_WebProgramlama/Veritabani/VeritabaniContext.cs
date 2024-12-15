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


        }
    }
}
