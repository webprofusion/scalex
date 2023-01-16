using Newtonsoft.Json;
using Scalex.UI.ViewModels;

using System.Runtime.InteropServices.JavaScript;

namespace Scalex.UI.Web
{
    public partial class SettingsProvider : IAppSettingsPprovider
    {
        public AppSettings LoadSettings()
        {
            var storedSettings = Interop.getLocalStorageItem("_scalexSettings");
            return JsonConvert.DeserializeObject<AppSettings>(storedSettings);
        }

        public void SaveSettings(AppSettings settings)
        {
            var json = JsonConvert.SerializeObject(settings);
            Interop.setLocalStorageItem("_scalexSettings", json);
        }
    }

    /// <summary>
    /// Implement JS interop to set/get local storage items
    /// </summary>
    internal static partial class Interop
    {
        public static string getLocalStorageItem(string name)
        {
            var value = _getLocalStorageItem(name);
            return value;
        }

        public static void setLocalStorageItem(string name, string value)
        {
            _setLocalStorageItem(name, value);
        }

        [JSImport("setLocalStorageItem", "./store.js")]
        internal static partial void _setLocalStorageItem(string name, string value);

        [JSImport("getLocalStorageItem", "./store.js")]
        internal static partial string _getLocalStorageItem(string name);
    }
}
