using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private List<AiModelConfig> aiModelConfigs = new List<AiModelConfig>();

        /// <summary>
        /// 行
        /// </summary>
        [ObservableProperty]
        private int rowCount;

        /// <summary>
        /// 列
        /// </summary>
        [ObservableProperty]
        private int cloumnCount;

        [ObservableProperty]
        private string cueWord = "AIMuster - ";
        #endregion
        #region Command
        [RelayCommand]
        private void Loaded() 
        {
            _messageService.ShowMessage("loaded");
        }


        [RelayCommand]
        private void Set()
        {
            var setWindow = WindowFactory.ShowWindow<SetWindow>(ShowMode.ShowDialog);

        }

        #endregion
    }
}
