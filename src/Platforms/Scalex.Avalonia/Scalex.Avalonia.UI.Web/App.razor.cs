using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace Scalex.Avalonia.UI.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        
        WebAppBuilder.Configure<Scalex.Avalonia.UI.App>()
            .UseReactiveUI()
            .SetupWithSingleViewLifetime();
    }
}