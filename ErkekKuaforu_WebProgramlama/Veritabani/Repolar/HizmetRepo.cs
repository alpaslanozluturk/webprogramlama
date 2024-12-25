using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.EntityFrameworkCore;

namespace ErkekKuaforu_WebProgramlama.Veritabani.Repolar
{
    public class HizmetRepo : GenericRepo<Hizmet>, IHizmetRepo
    {
        private readonly VeritabaniContext veritabani;
        private readonly DbSet<Hizmet> _dbset;
        public HizmetRepo(VeritabaniContext dbContext) : base(dbContext)
        {
            veritabani = dbContext;
            _dbset = veritabani.Set<Hizmet>();
        }
    }
}
