namespace Scalex.UI.ViewModels
{

    public class AppSettings
    {
        public int? SelectedScale { get; set; }
        public int? SelectedTuning { get; set; }

        public string? SelectedKey { get; set; }
    }

    public interface IAppSettingsPprovider
    {
        public AppSettings LoadSettings();
        public void SaveSettings(AppSettings settings);
    }
}