using MongoDB.Driver;
using System.Linq.Expressions;

namespace AgoraCertaminaBack.Data.Repository
{
    public interface IMongoRepository<T>
    {
        IQueryable<T> AsQueryable();

        IQueryable<T> FilterAsQueryable(Expression<Func<T, bool>> filterExpression);

        IEnumerable<T> FilterBy(Expression<Func<T, bool>> filterExpression);

        Task<IEnumerable<T>> FilterByAsync(Expression<Func<T, bool>> filterExpression);

        TProjected FilterBy<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression);

        Task<TProjected> FilterByAsync<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression);

        List<T> GetAll();

        Task<List<T>> GetAllAsync();

        T FindOne(Expression<Func<T, bool>> filterExpression);

        Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression);

        T FindById(string id);

        Task<T> FindByIdAsync(string id);

        void InsertOne(T document);

        Task InsertOneAsync(T document);

        void InsertMany(ICollection<T> documents);

        Task InsertManyAsync(ICollection<T> documents);

        void ReplaceOne(T document);

        Task ReplaceOneAsync(T document);

        void DeleteOne(Expression<Func<T, bool>> filterExpression);

        Task DeleteOneAsync(Expression<Func<T, bool>> filterExpression);

        void DeleteById(string id);

        Task DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<T, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<T, bool>> filterExpression);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition
    );
    }
}
