using System.Collections.Generic;

namespace RoomRes.Domain.Interfaces {
    public interface IBaseService<TType> where TType : class {
        Task<TType> GetByIdAsync(string id);
        Task AddAsync(TType entity);
        Task UpdateAsync(TType entity);
        Task DeleteAsync(TType entity);
        Task<List<TType>> GetAllAsync();
        Task<string> GetNextIdAsync();
    }
}