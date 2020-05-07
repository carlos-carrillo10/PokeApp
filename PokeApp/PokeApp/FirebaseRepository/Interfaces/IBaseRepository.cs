using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.FireBaseRepository.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetData<T>();
        Task<T> GetDataById(int id);
        Task<bool> SaveData<T>(T value);
        Task<bool> SaveDataRange<T>(IEnumerable<T> values);
        Task<bool> UpdateData<T>(T value);
        Task<bool> UpdateDataRange<T>(IEnumerable<T> values);
        Task<bool> DeleteData(int id);
        Task<int> GetLastID();

    }
}
