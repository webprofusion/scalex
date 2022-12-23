using Avalonia;
using Avalonia.Web.Blazor;

namespace Scalex.UI.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        AppBuilder.Configure<Scalex.UI.App>()
            .UseBlazor()
            .SetupWithSingleViewLifetime();

        base.OnParametersSet();
    }
}