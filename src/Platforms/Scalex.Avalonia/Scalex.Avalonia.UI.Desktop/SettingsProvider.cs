using Newtonsoft.Json;
using Scalex.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scalex.UI.Windows
{
    public partial class SettingsProvider : IAppSettingsPprovider
    {
        public AppSettings LoadSettings()
        {
            return new AppSettings();
        }

        public void SaveSettings(AppSettings settings)
        {
           
        }
    }
}
