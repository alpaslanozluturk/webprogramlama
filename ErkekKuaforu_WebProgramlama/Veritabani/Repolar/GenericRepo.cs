using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ErkekKuaforu_WebProgramlama.Veritabani.Repolar
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly VeritabaniContext _context;
        internal DbSet<T> dbSet;

        public GenericRepo(VeritabaniContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public IEnumerable<T> GetAll(string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.ToList();
        }

        public IEnumerable<T> GetAllByCondition(Expression<Func<T, bool>> filter, string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            request = request.Where(filter);
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.ToList();
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public T GetByIdWithProps(Expression<Func<T, bool>> filter, string? includeProps = null)
        {
            IQueryable<T> request = dbSet;
            request = request.Where(filter);
            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    request = request.Include(prop);
                }
            }
            return request.SingleOrDefault();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
