using Azure;
using hw6.Data;
using hw6.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace hw6.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IDatabaseEntity 
    {
        private readonly ExpensesDbContext _context;
        private DbSet<TEntity> Entities { get; set; }

        public EntityRepository(ExpensesDbContext context)
        {
            _context = context;
            Entities = _context.Set<TEntity>();
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await Entities.AddAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            TEntity? entity = await FindByIdAsync(id);
            if (entity != null)
            {
                Entities.Remove(entity);
                return true;
            }
            return false;
        }

        public IQueryable<TEntity> GetAllEntities()
        {
            return Entities;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<int> TotalCountOfEntitiesAsync()
        {
            return Entities.CountAsync();
        }

        public bool Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Update(entity);
            return true;
        }

        public Task<TEntity?> FindByIdAsync(int id)
        {
            return Entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<TEntity> GetEntititesByPage(int page, int pageSize)
        {
            if (page < 0 || pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(page));
            int ExcludeRecords = (pageSize * page) - pageSize;
            return  Entities.OrderBy(x => x.Id).Skip(ExcludeRecords).Take(pageSize);
        }

        public IQueryable<TEntity> GetJoinEntities(params string[] columnsToJoin)
        {
            IQueryable<TEntity> query = Entities;
            foreach (var column in columnsToJoin)
            {
                query = query.Include(column);
            }

            return query;
        }
    }
}