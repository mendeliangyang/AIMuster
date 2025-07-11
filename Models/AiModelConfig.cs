using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AIMuster.Models
{
   public partial class AiModelConfig : ObservableObject
    {
        /// <summary>
        /// 模型名称
        /// </summary>
        [ObservableProperty]
        private string modelName;
        /// <summary>
        /// 模型id
        /// </summary>
        [ObservableProperty]
        private string modelId;
        [ObservableProperty] 
        private string modelType ;
        [ObservableProperty] 
        private string modelVersion ;
        [ObservableProperty] 
        private string modelDescription ;
        [ObservableProperty] 
        private string modelUrl ;
        [ObservableProperty] 
        private string modelIconUrl ;
        [ObservableProperty] 
        private bool isEnabled  = false;
        [ObservableProperty] 
        private bool isDefault  = false;
        [ObservableProperty] 
        private bool isCustomModel  = false;

        [ObservableProperty]
        private int rowIndex =-1;

        [ObservableProperty]
        private int columnIndex =-1;

        /// <summary>
        /// 填充元素的JavaScript代码
        /// </summary>
        [ObservableProperty]
        private string obtainElementJs;

    }
}
