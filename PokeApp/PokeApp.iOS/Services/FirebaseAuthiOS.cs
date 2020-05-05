using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;
using PokeApp.iOS.Services;
using PokeApp.Services.Interfaces;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthiOS))]
namespace PokeApp.iOS.Services
{
    public class FirebaseAuthiOS : IFirebaseAuth
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}