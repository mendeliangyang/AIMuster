using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AIMuster.Message;
using AIMuster.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace AIMuster.UI
{
    /// <summary>
    /// SetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SetWindow : Window
    {
        public SetWindow()
        {
            InitializeComponent();

            WeakReferenceMessenger.Default.Register<SetWindow, CloseWindowMessage>(this, (recipient, message) =>
            {
                recipient.Close();
                System.Diagnostics.Debug.WriteLine($"窗口已关闭 (通过指定发送者过滤)。");
            });
        }
    }
}
