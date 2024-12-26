using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.EntityFrameworkCore;

namespace ErkekKuaforu_WebProgramlama.Veritabani.Repolar
{
    public class RandevuRepo : GenericRepo<Randevu>, IRandevuRepo
    {
        private readonly VeritabaniContext veritabani;
        private readonly DbSet<Randevu> _dbset;
        public RandevuRepo(VeritabaniContext dbContext) : base(dbContext)
        {
            veritabani = dbContext;
            _dbset = veritabani.Set<Randevu>();
        }
    }
}
