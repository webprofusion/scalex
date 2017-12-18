using Foundation;
using UIKit;

namespace Scalex.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            var appInstance = new App();
            appInstance.DisplayScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;
            appInstance.DisplayScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;
            appInstance.DisplayScaleFactor = (double)UIScreen.MainScreen.Scale;

            LoadApplication(appInstance);

            var x = typeof(Xamarin.Forms.Themes.DarkThemeResources);
            x = typeof(Xamarin.Forms.Themes.LightThemeResources);
            x = typeof(Xamarin.Forms.Themes.iOS.UnderlineEffect);

            return base.FinishedLaunching(app, options);
        }
    }
}