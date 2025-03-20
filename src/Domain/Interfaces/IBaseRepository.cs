using System.Collections.Generic;
using RoomRes.Domain.Models;

namespace RoomRes.Domain.Interfaces {
    public interface IBaseRepository<TType> where TType : BaseModel {
        Task<TType> GetByIdAsync(string id);
        Task AddAsync(TType entity);
        Task UpdateAsync(TType entity);
        Task DeleteAsync(TType entity);
        Task<List<TType>> GetAllAsync();
        Task<string> GetNextIdAsync();
        Task<TType> GetByFieldAsync<TField>(string fieldName, TField value);
    }
}