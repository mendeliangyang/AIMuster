using System;
using System.Collections.ObjectModel;
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
            var configs = ConfigManager.LoadAiModelConfig();
            var viewConfigs = ConfigManager.LoadViewAiModelConfig();
            var cellCount = RowCount * ColumnCount;
            AiModelConfigs.Clear();
            int cellIndex = -1;
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    cellIndex ++;
                    var viewModel = viewConfigs.FirstOrDefault(a => a.RowIndex == i && a.ColumnIndex == j);
                    if (viewModel != null) 
                    {
                        AiModelConfigs.Add(viewModel);
                        continue;
                    }
                    if (viewConfigs.Count > cellIndex)
                    {
                        viewModel = viewConfigs.FirstOrDefault(a => !AiModelConfigs.Any(b => b.ModelId == a.ModelId));
                        if (viewModel!=null)
                        {
                            AiModelConfigs.Add(viewModel);
                            continue;
                        }
                    }
                    AiModelConfigs.Add(new AiModelConfig());
                }
            }

        }


        [RelayCommand]
        private async Task KeyEnter()
        {
            //Debug.WriteLine($"按下了回车，内容是：{CueWord}");

            if (!string.IsNullOrEmpty(CueWord))
            {
                foreach (var model in AiModelConfigs)
                {
                    if (!model.IsEnabled&&model.TargetWebView==null)
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

        #endregion
    }
}
