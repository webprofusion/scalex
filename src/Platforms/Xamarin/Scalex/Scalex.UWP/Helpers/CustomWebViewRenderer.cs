using Scalex.Helpers;
using Scalex.UWP.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(CustomWebViewRender))]

namespace Scalex.UWP.Helpers
{
    public class CustomWebViewRender : WebViewRenderer
    {
        private bool _scriptNotifyAttached = false;
        private string _scriptNotifyResult = null;

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (!_scriptNotifyAttached)
            {
                ((Windows.UI.Xaml.Controls.WebView)Control).ScriptNotify += CustomWebViewRender_ScriptNotify;
                _scriptNotifyAttached = true;
            }

            var webView = e.NewElement as CustomWebView;
            if (webView != null)
                webView.EvaluateJavascript = async (js) =>
                {
                    _scriptNotifyResult = null;
                    return await Control.InvokeScriptAsync("eval", new[] { js });
                };
        }

        private void CustomWebViewRender_ScriptNotify(object sender, Windows.UI.Xaml.Controls.NotifyEventArgs e)
        {
            _scriptNotifyResult = e.Value;
        }
    }
}