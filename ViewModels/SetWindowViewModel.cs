using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        [ObservableProperty]
        AppConfig appConfig;


        [ObservableProperty]
        private bool ifChanged=false;


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
            ConfigManager.Save(AppConfig);
        }

        [RelayCommand]
        private void SaveConfigAndClose()
        {
            SaveConfig();

            // 发送一个普通的关闭消息
            WeakReferenceMessenger.Default.Send(new CloseWindowMessage(), this);
            WeakReferenceMessenger.Default.Send(SetWindowViewModel.CloseWindowMessage, this);
        }
        
    }
}
