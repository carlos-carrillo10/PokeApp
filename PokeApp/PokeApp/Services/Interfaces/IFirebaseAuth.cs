using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PokeApp.Services.Interfaces
{
    public interface IFirebaseAuth
    {
        Task<Dictionary<string, string>> LoginWithEmailPassword(string email, string password);
    }
}
