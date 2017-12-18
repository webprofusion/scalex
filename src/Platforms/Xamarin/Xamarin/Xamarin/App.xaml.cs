using Scalex.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Scalex
{
    public partial class App : Application
    {
        public double DisplayScreenHeight { get; set; }
        public double DisplayScreenWidth { get; set; }
        public double DisplayScaleFactor { get; set; }

        public App()
        {
            InitializeComponent();

            SetMainPage();

            PopulateScreenDimensions();
        }

        public void PopulateScreenDimensions()
        {
            // https://stackoverflow.com/questions/41489532/density-of-screen-in-ios-and-universal-windows-app/41510448#41510448
#if WINDOWS_UWP
                DisplayScreenHeight = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Height;
                DisplayScreenWidth = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().VisibleBounds.Width;

                // Size of screen's resolution
                //screensize.App.DisplayScreenWidth = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().ScreenHeightInRawPixels;
                //screensize.App.DisplayScreenHeight = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().ScreenWidthInRawPixels;

                // Pixels per View Pixel
                // - https://msdn.microsoft.com/en-us/windows/uwp/layout/design-and-ui-intro#effective-pixels-and-scaling
                // - https://msdn.microsoft.com/en-us/windows/uwp/layout/screen-sizes-and-breakpoints-for-responsive-design
                DisplayScaleFactor = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

#endif

#if __ANDROID__
            // DisplayScreenWidth = Display. / (double)Resources.DisplayMetrics.Density;
            // DisplayScreenHeight = (double)Resources.DisplayMetrics.HeightPixels /
            // (double)Resources.DisplayMetrics.Density; DisplayScaleFactor = (double)Resources.DisplayMetrics.Density;
#endif

#if __IOS__
            // Store off the device sizes, so we can access them within Xamarin Forms
            // DisplayScreenWidth = (double)UIScreen.MainScreen.Bounds.Width; DisplayScreenHeight =
            // (double)UIScreen.MainScreen.Bounds.Height; DisplayScaleFactor = (double)UIScreen.MainScreen.Scale;
#endif
        }

        public static void SetMainPage()
        {
            /*Current.MainPage = new TabbedPage
            {
                Children =
                {
                    new NavigationPage(new ItemsPage())
                    {
                        Title = "Browse",
                        Icon = Device.OnPlatform<string>("tab_feed.png",null,null)
                    },
                    new NavigationPage(new AboutPage())
                    {
                        Title = "About",
                        Icon = Device.OnPlatform<string>("tab_about.png",null,null)
                    },
                }
            };*/
            Current.MainPage = new NavigationPage(new IntroPage())
            {
                Title = "Guitar Toolkit"
            };
        }
    }
}