namespace ShiftFlow.Application.Common.Interfaces.Repositories
{
    public interface IRepository<TEntity>
    {
        Task Add(TEntity entity);
        Task AddRange(List<TEntity> entities);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        Task<TEntity> GetById(int id);
        Task<List<TEntity>> GetAll();
    }
}
