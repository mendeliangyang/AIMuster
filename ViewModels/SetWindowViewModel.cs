using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIMuster.Config;
using AIMuster.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AIMuster.ViewModels
{
    partial class SetWindowViewModel :ObservableObject
    {

        AppConfig _appConfig;
        public SetWindowViewModel(AppConfig appConfig)
        {
            _appConfig = appConfig;
            rowCount = _appConfig.RowCount;
            columnCount = _appConfig.ColumnCount;
            language = _appConfig.Language;
        }

        [ObservableProperty]
        private int rowCount =1;

        [ObservableProperty]
        private int columnCount = 1;

        [ObservableProperty]
        private string language;


        [RelayCommand]
        private void LanguageChanged(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                LanguageManager.ChangeLanguage(language);
            }
            _appConfig.Language = language;
            ConfigManager.Save(_appConfig);
        }
        [RelayCommand]
        private void SaveConfig()
        {
            _appConfig.RowCount = RowCount;
            _appConfig.ColumnCount = ColumnCount;
            ConfigManager.Save(_appConfig);
        }
    }
}
