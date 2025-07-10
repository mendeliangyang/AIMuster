using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIMuster.Config;
using AIMuster.Models;
using AIMuster.Services;
using AIMuster.UI;
using AIMuster.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AIMuster.ViewModels
{
    public partial class MainWindowViewModel :ObservableObject
    {
        IMessageService _messageService;
        public MainWindowViewModel( IMessageService messageService) 
        {
            _messageService = messageService;
        }

        #region Data

        /// <summary>
        /// 模型列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<AiModelConfig> aiModelConfigs = new ObservableCollection<AiModelConfig>();

        /// <summary>
        /// 行
        /// </summary>
        [ObservableProperty]
        private int rowCount;

        /// <summary>
        /// 列
        /// </summary>
        [ObservableProperty]
        private int columnCount;

        [ObservableProperty]
        private string cueWord = "AIMuster - ";
        #endregion
        #region Command
        [RelayCommand]
        private void Loaded() 
        {
            //_messageService.ShowMessage("loaded");
            Reload();
        }


        [RelayCommand]
        private void Set()
        {
            var setWindow = WindowFactory.ShowWindow<SetWindow>(ShowMode.ShowDialog);
            Reload();
        }


        [RelayCommand]
        private void Reload()
        {
            AiModelConfigs.Clear();
            var appConfig = ConfigManager.LoadAppConfig();
            RowCount = appConfig.RowCount;
            ColumnCount = appConfig.ColumnCount;
            var configs = ConfigManager.LoadAiModelConfig();
            var viewConfigs = ConfigManager.LoadViewAiModelConfig();
            var cellCount = RowCount * ColumnCount;

            for (int i = 0; i < cellCount; i++)
            {
                AiModelConfigs.Add(new AiModelConfig());
            }
        }

        #endregion
    }
}
