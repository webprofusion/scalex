namespace Scalex.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            LoadApplication(new Scalex.App());
        }
    }
}