using System.Collections.Generic;
using System.Linq;
using RoomRes.Domain.Interfaces;
using RoomRes.Domain.Models;

namespace RoomRes.Application.Services {
    public class BaseService<TType> : IBaseService<TType> where TType : BaseModel {
        private readonly IBaseRepository<TType> _repository;

        public BaseService(IBaseRepository<TType> repository) {
            _repository = repository;
        }

        public async Task<TType> GetByIdAsync(string id) {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(TType entity) {
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(TType entity) {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(TType entity) {
            await _repository.DeleteAsync(entity);
        }

        public async Task<List<TType>> GetAllAsync() {
            return (await _repository.GetAllAsync()).OrderBy(r => {
                    if (int.TryParse(r.Id, out int id)) return id;
                    else return int.MaxValue; })
                .ToList();
        }

        public async Task<string> GetNextIdAsync() {
            return await _repository.GetNextIdAsync();
        }
    }
}