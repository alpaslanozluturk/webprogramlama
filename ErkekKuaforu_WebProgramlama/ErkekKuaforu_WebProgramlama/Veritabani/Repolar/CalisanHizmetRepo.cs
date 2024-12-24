using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.EntityFrameworkCore;

namespace ErkekKuaforu_WebProgramlama.Veritabani.Repolar
{
    public class CalisanHizmetRepo : GenericRepo<CalisanHizmet>, ICalisanHizmetRepo
    {
        private readonly VeritabaniContext veritabani;
        private readonly DbSet<CalisanHizmet> _dbset;
        public CalisanHizmetRepo(VeritabaniContext dbContext) : base(dbContext)
        {
            veritabani = dbContext;
            _dbset = veritabani.Set<CalisanHizmet>();
        }
    }
}
