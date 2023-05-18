namespace hw6.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IDatabaseEntity
    {
        Task<int> TotalCountOfEntitiesAsync();
        IQueryable<TEntity> GetAllEntities();
        Task<bool> AddAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);
        bool Update(TEntity entity);
        Task<int> SaveChangesAsync();
        IQueryable<TEntity> GetJoinEntities(params string[] columnsToJoin);
        IQueryable<TEntity> GetEntititesByPage(int page, int pageSize);
        Task<TEntity?> FindByIdAsync(int id);
    }
}
