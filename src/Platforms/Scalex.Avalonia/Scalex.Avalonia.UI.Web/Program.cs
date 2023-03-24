using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using System.Threading.Tasks;
using System.Runtime.InteropServices.JavaScript;
using Scalex.UI;
using Scalex.UI.Web;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var appBuilder = await BuildAvaloniaApp();

        await appBuilder.UseReactiveUI()
                .StartBrowserAppAsync("out");
    }
    public static async Task<AppBuilder> BuildAvaloniaApp()
    {
        await JSHost.ImportAsync("./store.js", "./store.js");

        return AppBuilder.Configure<App>(() =>
        {
            var app = new App(new SettingsProvider());
            return app;
        });
    }

}