using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Scalex.Helpers
{
    public class CustomWebView : WebView
    {
        //https://xamarinhelp.com/xamarin-forms-webview-executing-javascript/

        public static BindableProperty EvaluateJavascriptProperty =
        BindableProperty.Create(nameof(EvaluateJavascript), typeof(Func<string, Task<string>>), typeof(CustomWebView), null, BindingMode.OneWayToSource);

        public Func<string, Task<string>> EvaluateJavascript
        {
            get { return (Func<string, Task<string>>)GetValue(EvaluateJavascriptProperty); }
            set { SetValue(EvaluateJavascriptProperty, value); }
        }
    }
}