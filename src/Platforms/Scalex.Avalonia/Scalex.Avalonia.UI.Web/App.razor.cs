using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace Scalex.UI.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        
        WebAppBuilder.Configure<Scalex.UI.App>()
            .UseReactiveUI()
            .SetupWithSingleViewLifetime();
    }
}