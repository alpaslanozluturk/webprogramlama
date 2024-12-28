using ErkekKuaforu_WebProgramlama.Models;
using Microsoft.EntityFrameworkCore;

namespace ErkekKuaforu_WebProgramlama.Veritabani.Repolar
{
    public class ApiRepo : GenericRepo<ApiModel>, IApiRepo
    {
        private readonly VeritabaniContext veritabani;
        private readonly DbSet<ApiModel> _dbset;
        public ApiRepo(VeritabaniContext dbContext) : base(dbContext)
        {
            veritabani = dbContext;
            _dbset = veritabani.Set<ApiModel>();
        }
    }
}
