using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AIMuster.Config;
using AIMuster.Message;
using AIMuster.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace AIMuster.ViewModels
{
    partial class SetWindowViewModel :ObservableObject
    {

        public SetWindowViewModel(AppConfig config)
        {
            appConfig = config;
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }

        [ObservableProperty]
        AppConfig appConfig;


        [ObservableProperty]
        private bool ifChanged=false;

        [ObservableProperty]
        private string version = "";


        [RelayCommand]
        private void LanguageChanged(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                LanguageManager.ChangeLanguage(language);
            }
            IfChanged = true;
            ConfigManager.Save(AppConfig);
        }
        [RelayCommand]
        private void SaveConfig()
        {
            IfChanged = true;

            //切换主题
            ThemeManager.SwitchTheme(AppConfig.Theme.ToString());
            ConfigManager.Save(AppConfig);
        }

        [RelayCommand]
        private void CloseWindow()
        {
            //SaveConfig();
            // 发送一个普通的关闭消息
            WeakReferenceMessenger.Default.Send(new CloseWindowMessage());
        }
        
    }
}
