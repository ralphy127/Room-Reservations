using System.Collections.Generic;
using MongoDB.Driver;
using RoomRes.Domain.Interfaces;
using RoomRes.Domain.Models;

namespace RoomRes.Infrastructure.Repositories {
    public class BaseRepository<TType> : IBaseRepository<TType> where TType : BaseModel {
        private readonly IMongoCollection<TType> _collection;

        public BaseRepository(IMongoDatabase database, string collectionName) {
            _collection = database.GetCollection<TType>(collectionName);
        }

        public async Task<List<TType>> GetAllAsync() {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<TType> GetByIdAsync(string id) {
            FilterDefinition<TType> filter = Builders<TType>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TType entity) {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(TType entity) {
            FilterDefinition<TType> filter = Builders<TType>.Filter.Eq("_id", (entity as dynamic).Id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(TType entity) {
            FilterDefinition<TType> filter = Builders<TType>.Filter.Eq("_id", (entity as dynamic).Id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<string> GetNextIdAsync() {
            List<string?> existingIds = await _collection.Find(Builders<TType>.Filter.Empty)
                                    .Project(x => x.Id).ToListAsync();

            int nextId = 1; 
            foreach (string? id in existingIds) {
                if (id != nextId.ToString()) {
                    break; 
                }

                nextId++; 
            }

            return nextId.ToString(); 
        }

        public async Task<TType> GetByFieldAsync<TField>(string fieldName, TField val) {
            FilterDefinition<TType> filter = Builders<TType>.Filter.Eq(fieldName, val);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}