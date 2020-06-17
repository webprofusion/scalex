using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;

namespace Scalex.Droid
{
    [Activity(Label = "Guitar Toolkit", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            // global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");

            // image resource loading
            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var app = new App();
            app.DisplayScreenWidth = (double)Resources.DisplayMetrics.WidthPixels / (double)Resources.DisplayMetrics.Density;
            app.DisplayScreenHeight = (double)Resources.DisplayMetrics.HeightPixels / (double)Resources.DisplayMetrics.Density;
            app.DisplayScaleFactor = (double)Resources.DisplayMetrics.Density;

            LoadApplication(app);
        }
    }
}