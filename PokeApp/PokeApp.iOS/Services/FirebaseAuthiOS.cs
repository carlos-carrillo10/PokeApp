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
        public async Task<Dictionary<string, string>> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                if (user != null)
                {
                    var dic = new Dictionary<string, string>();
                    dic.Add("Token", await user.User.GetIdTokenAsync());
                    dic.Add("UserId", user.User.Uid);
                    return dic;
                }
                else
                    return default(Dictionary<string, string>);
            }
            catch (Exception e)
            {
                return default(Dictionary<string, string>);
            }
        }
    }
}