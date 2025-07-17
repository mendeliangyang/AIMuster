using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        IAppService _appService;

        public IRelayCommand<KeyEventArgs> EnterCommand { get; }

        public MainWindowViewModel( IMessageService messageService,IAppService appService) 
        {
            _appService = appService;
            _messageService = messageService;

            EnterCommand = new RelayCommand<KeyEventArgs>(OnEnter);
        }

        #region Data

        /// <summary>
        /// 模型列表
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<AiModelConfig> aiViewModelConfigs = new ObservableCollection<AiModelConfig>();


        [ObservableProperty]
        private List<AiModelConfig> aiModelConfigs = ConfigManager.LoadAiModelConfig();


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
            var swVm = setWindow.DataContext as SetWindowViewModel;
            if (swVm?.IfChanged==true)
            {
                Reload();
            }
        }


        [RelayCommand]
        private void Reload()
        {
            var appConfig = ConfigManager.LoadAppConfig();
            RowCount = appConfig.RowCount;
            ColumnCount = appConfig.ColumnCount;
            var viewConfigs = ConfigManager.LoadViewAiModelConfig();
            var cellCount = RowCount * ColumnCount;
            AiViewModelConfigs.Clear();
            int cellIndex = -1;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    cellIndex ++;
                    var viewModel = viewConfigs.FirstOrDefault(a => a.RowIndex == i && a.ColumnIndex == j);
                    if (viewModel != null) 
                    {
                        viewModel.RowIndex = i;
                        viewModel.ColumnIndex = j;
                        AiViewModelConfigs.Add(viewModel);
                        continue;
                    }
                    if (viewConfigs.Count > cellIndex)
                    {
                        viewModel = viewConfigs.FirstOrDefault(a => !AiViewModelConfigs.Any(b => b.ModelId == a.ModelId));
                        if (viewModel!=null)
                        {
                            viewModel.RowIndex = i;
                            viewModel.ColumnIndex = j;
                            AiViewModelConfigs.Add(viewModel);
                            continue;
                        }
                    }
                    AiViewModelConfigs.Add(new AiModelConfig());
                }
            }

            //保存配置文件
            ConfigManager.SaveAiModelConfig(AiModelConfigs);
            ConfigManager.SaveViewAiModelConfig(AiViewModelConfigs.ToList());
        }


        [RelayCommand]
        private async Task KeyEnter()
        {
            if (!string.IsNullOrEmpty(CueWord))
            {
                foreach (var model in AiViewModelConfigs)
                {
                    if (!model.IsValid|| !model.IsEnabled||model.TargetWebView==null)
                    {
                        continue;
                    }
                    var runJs = model.ObtainElementJs.Replace(ConfigManager.PromptCodeWeb, CueWord);
                    //通过 webview2 运行js 填充
                    await model.TargetWebView?.ExecuteScriptAsync(runJs);
                    model.TargetWebView?.ExecuteScriptAsync(model.SendElementJs);
                }
                CueWord = "";
            }
        }

        [RelayCommand]
        private void ExitApp()
        {
            _appService.Shutdown();
        }

        private void OnEnter(KeyEventArgs e)
        {
            if (e?.Key == Key.Enter)
            {
                KeyEnter();
            }
        }

        [RelayCommand]
        private void MouseEnter(AiModelConfig aiModelConfig) => aiModelConfig.IsMouseOver = true;

        [RelayCommand]
        private void MouseLeave(AiModelConfig aiModelConfig) => aiModelConfig.IsMouseOver = false;

        [RelayCommand]
        private void SelectModel(object param)
        {
            dynamic p = param!;
            var selected = p.SelectedItem as AiModelConfig;
            var itemData = p.CurrentDataItem as AiModelConfig;
            var combobox = p.ComboBox as ComboBox;
            //selected = AiModelConfigs[combobox.SelectedIndex];

            if (selected != null && itemData != null && itemData.TargetWebView != null)
            {
                var selectViewModel = AiViewModelConfigs.FirstOrDefault(a=>a.ModelId==selected.ModelId);
                if (selectViewModel!=null)
                {
                    var selectViewModelIndex = AiViewModelConfigs.IndexOf(selectViewModel);
                    AiViewModelConfigs.Remove(selectViewModel);
                    AiViewModelConfigs.Insert(selectViewModelIndex,new AiModelConfig());
                }

                var model = AiViewModelConfigs.FirstOrDefault(a=>a.ModelId==itemData.ModelId);
                var modelIndex = -1;
                if (model!=null)
                {
                    modelIndex = AiViewModelConfigs.IndexOf(model);
                    AiViewModelConfigs.Remove(model);
                }
                if (modelIndex!=-1)
                {
                    selected.RowIndex = model.RowIndex;
                    selected.ColumnIndex = model.ColumnIndex;
                    AiViewModelConfigs.Insert(modelIndex, selected);
                }
                ConfigManager.SaveViewAiModelConfig(AiViewModelConfigs.ToList());
            }


        }

        [RelayCommand]
        private void RefreshWeb(AiModelConfig aiModel)
        {
            if (aiModel?.TargetWebView != null)
            {
                aiModel.TargetWebView.Reload();
            }
            else
            {
                _messageService.ShowMessage("WebView未初始化或不存在");
            }
        }


            #endregion
        }
}
