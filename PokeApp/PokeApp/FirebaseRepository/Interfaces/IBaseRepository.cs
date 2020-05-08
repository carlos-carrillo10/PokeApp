using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.FireBaseRepository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllData(string UserId, string Region);
        Task<T> GetDataById(int id, string UserId, string Region);
        Task<T> GetDataByName(string Name, string UserId, string Region);
        Task<IEnumerable<T>> GetAllDataById(int id, string UserId, string Region);
        Task<IEnumerable<T>> GetAllDataByName(string Name, string UserId);
        Task<bool> SaveData(T value);
        Task<bool> SaveDataRange(IEnumerable<T> values);
        Task<bool> UpdateData(T value);
        Task<bool> UpdateDataRange(IEnumerable<T> values);
        Task<bool> DeleteData(int id, string UserId, string Region);
        Task<int> GetLastID(string UserId, string Region);

    }
}
