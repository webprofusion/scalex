using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.Web;
using Scalex.UI;
using Scalex.UI.Web;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;
using System.Threading.Tasks;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
    private static async Task Main(string[] args)
    {
        var appBuilder = await BuildAvaloniaApp();

        appBuilder
            .UseReactiveUI()
            .SetupBrowserApp("out");
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