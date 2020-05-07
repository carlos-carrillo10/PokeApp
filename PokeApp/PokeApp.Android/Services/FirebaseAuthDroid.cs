using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using PokeApp.Droid.Services;
using PokeApp.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthDroid))]
namespace PokeApp.Droid.Services
{
    public class FirebaseAuthDroid : IFirebaseAuth
    {
        public async Task<Dictionary<string, string>> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                if (user != null)
                {
                    var dic = new Dictionary<string, string>();
                    dic.Add("Token", (await user.User.GetIdTokenAsync(false)).Token);
                    dic.Add("UserId", user.User.Uid);
                    return dic;
                }
                else
                    return default(Dictionary<string, string>);
              
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return default(Dictionary<string, string>);
            }
        }
    }
}