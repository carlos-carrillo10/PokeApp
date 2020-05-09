using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.Ioc;

namespace PokeApp.Droid
{
    [Activity(Label = "PokeApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static FirebaseApp app;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            UserDialogs.Init(this);

            AppCenter.Start("5e140adf-f042-4958-b98b-d832a82a4d21",
                   typeof(Analytics), typeof(Crashes));

            var options = new FirebaseOptions.Builder()
                    .SetApplicationId("1:829430273380:android:1cbf564db96124d1be5671")
                    .SetApiKey("AIzaSyDrI3b9SSN6sw-YJayIhFbFvITk-HnMI-k")
                    .SetDatabaseUrl("https://pokeapp-276302.firebaseio.com")
                    .Build();

            if (app == null)
                app = FirebaseApp.InitializeApp(this, options);

            LoadApplication(new App(new AndroidInitializer()));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

