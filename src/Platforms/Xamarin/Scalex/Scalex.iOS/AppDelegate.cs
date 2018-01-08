using FFImageLoading.Forms.Touch;
using FFImageLoading.Svg.Forms;
using Foundation;
using UIKit;

namespace Scalex.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            // image resource loading
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);

            global::Xamarin.Forms.Forms.Init();

            var appInstance = new App();
            appInstance.DisplayScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;
            appInstance.DisplayScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;
            appInstance.DisplayScaleFactor = (double)UIScreen.MainScreen.Scale;

            LoadApplication(appInstance);

            return base.FinishedLaunching(app, options);
        }
    }
}