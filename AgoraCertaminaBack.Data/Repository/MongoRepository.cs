using AgoraCertaminaBack.Data.Settings;
using AgoraCertaminaBack.Models.Entities.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace AgoraCertaminaBack.Data.Repository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly string _customerId;

        public MongoRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            var collectionName = typeof(T).Name.ToLowerInvariant();

            _collection = database.GetCollection<T>(collectionName);
            _customerId = string.Empty;
        }

        private FilterDefinition<T> ApplyGlobalFilter(FilterDefinition<T> filter)
        {
            // Filtro global para verificar si el registro está activo
            var isActiveFilter = Builders<T>.Filter.Eq(doc => doc.IsActive, true);

            // Aplica el filtro global primero
            filter = Builders<T>.Filter.And(filter, isActiveFilter);

            // Si T implementa ICustomerAttribute, aplica el filtro para CustomerId
            if (typeof(ICustomerAttribute).IsAssignableFrom(typeof(T)))
            {
                var customerFilter = Builders<T>.Filter.Eq(doc => ((ICustomerAttribute)(object)doc).OrganizationId, _customerId);

                // Combina el filtro global con el filtro de CustomerId
                filter = Builders<T>.Filter.And(filter, customerFilter);
            }

            return filter;
        }

        public virtual IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public IQueryable<T> FilterAsQueryable(Expression<Func<T, bool>> filterExpression)
        {
            return _collection.AsQueryable().Where(filterExpression);
        }

        public virtual IEnumerable<T> FilterBy(
            Expression<Func<T, bool>> filterExpression)
        {
            //var finalFilter = ApplyGlobalFilter(Builders<T>.Filter.Where(filterExpression));
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual async Task<IEnumerable<T>> FilterByAsync(
            Expression<Func<T, bool>> filterExpression)
        {
            //var finalFilter = ApplyGlobalFilter(Builders<T>.Filter.Where(filterExpression));
            return await _collection.Find(filterExpression).ToListAsync();
        }

        public virtual TProjected FilterBy<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression)
        {
            //var finalFilter = ApplyGlobalFilter(Builders<T>.Filter.Where(filterExpression));
            var query = _collection.Find(filterExpression).Project(projectionExpression);

            // Si el tipo proyectado es una colección, devolver como lista
            if (typeof(TProjected).IsGenericType &&
                typeof(TProjected).GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return (TProjected)(object)query.ToList();
            }

            // Si no, devolver un solo objeto
            return query.FirstOrDefault();
        }

        public virtual async Task<TProjected> FilterByAsync<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression)
        {
            //var finalFilter = ApplyGlobalFilter(Builders<T>.Filter.Where(filterExpression));

            var query = _collection.Find(filterExpression).Project(projectionExpression);

            // Si el tipo proyectado es una colección, usar ToListAsync
            if (typeof(TProjected).IsGenericType &&
                typeof(TProjected).GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return (TProjected)(object)await query.ToListAsync();
            }

            // Si no, devolver un solo objeto con FirstOrDefaultAsync
            return await query.FirstOrDefaultAsync();
        }

        public virtual List<T> GetAll()
        {
            return _collection.Find(_ => true).ToList();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public virtual T FindOne(Expression<Func<T, bool>> filterExpression)
        {
            //var finalFilter = ApplyGlobalFilter(Builders<T>.Filter.Where(filterExpression));

            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public async Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression)
        {
            //var finalFilter = ApplyGlobalFilter(Builders<T>.Filter.Where(filterExpression));

            return await _collection.Find(filterExpression).FirstOrDefaultAsync();
        }

        public virtual T FindById(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual async Task<T> FindByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            return await _collection.Find(filter).SingleOrDefaultAsync();
        }

        public virtual void InsertOne(T document)
        {
            _collection.InsertOne(document);
        }

        public virtual async Task InsertOneAsync(T document)
        {
            await _collection.InsertOneAsync(document);
        }

        public void InsertMany(ICollection<T> documents)
        {
            _collection.InsertMany(documents);
        }


        public virtual async Task InsertManyAsync(ICollection<T> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void ReplaceOne(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task ReplaceOneAsync(T document)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void DeleteOne(Expression<Func<T, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteOneAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            _collection.FindOneAndDelete(filter);
        }

        public async Task DeleteByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, id);
            await _collection.FindOneAndDeleteAsync(filter);
        }

        public void DeleteMany(Expression<Func<T, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> filterExpression)
        {
            await _collection.DeleteManyAsync(filterExpression);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).AnyAsync();
        }

        public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition)
        {
            return await _collection.UpdateOneAsync(filterDefinition, updateDefinition);
        }
    }
}
