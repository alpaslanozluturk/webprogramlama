using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.EntityFrameworkCore;

namespace ErkekKuaforu_WebProgramlama.Veritabani.Repolar
{
    public class CalisanRepo : GenericRepo<Calisan>, ICalisanRepo
    {
        private readonly VeritabaniContext veritabani;
        private readonly DbSet<Calisan> _dbset;
        public CalisanRepo(VeritabaniContext dbContext) : base(dbContext)
        {
            veritabani = dbContext;
            _dbset = veritabani.Set<Calisan>();
        }
    }
}
