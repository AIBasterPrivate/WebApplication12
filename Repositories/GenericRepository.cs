using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication12.Models;

namespace WebApplication12.Repositories
{
    public class GenericRepository<T>:IRepository<T> where T: ModelBase
    {
        private readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);

            return entity;
        }

        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, id);
            var result = await _collection.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetAsync(ObjectId id)
        {
            var filter = Builders<T>.Filter.Eq(e => e.Id, id);
            var entity = await _collection.Find(filter).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<bool> UpdateAsync(ObjectId id, T entity)
        {
            ReplaceOneResult result = await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity, new ReplaceOptions() { IsUpsert = true });

            return result.ModifiedCount == 1;
        }
    }
}
