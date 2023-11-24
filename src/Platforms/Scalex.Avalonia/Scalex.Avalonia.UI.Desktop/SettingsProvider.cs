using Scalex.UI.ViewModels;
using System;
using System.IO;

namespace Scalex.UI.Windows
{
    public partial class SettingsProvider : IAppSettingsPprovider
    {
        public string GetAppDataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Scalex");
        }

        public AppSettings LoadSettings()
        {
            var settingsPath = Path.Combine(GetAppDataPath(), "settings.json");

            if (File.Exists(settingsPath))
            {
                return System.Text.Json.JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(settingsPath));
            }
            return new AppSettings();
        }

        public void SaveSettings(AppSettings settings)
        {
            if (!Path.Exists(GetAppDataPath()))
            {
                Directory.CreateDirectory(GetAppDataPath());
            }

            var settingsPath = Path.Combine(GetAppDataPath(), "settings.json");
            File.WriteAllText(settingsPath, System.Text.Json.JsonSerializer.Serialize(settings));
        }
    }
}
