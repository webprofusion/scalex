using AppKit;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Scalex.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        private NSWindow window;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            window.Title = "Soundshed Guitar Toolkit";
            window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow
        {
            get { return window; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // image resource loading
            CachedImageRenderer.Init();
            var ignore = typeof(SvgCachedImage);

            Forms.Init();

            var appInstance = new App();
            appInstance.DisplayScreenWidth = 1024;
            appInstance.DisplayScreenHeight = 768;
            appInstance.DisplayScaleFactor = 2;

            LoadApplication(appInstance);

            base.DidFinishLaunching(notification);
        }
    }
}