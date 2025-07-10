using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIMuster.Services
{
    public class MessageService : IMessageService
    {
        public void ShowMessage(string message)
        {
            // 实现显示消息的逻辑
            Debug.WriteLine($"Message: {message}");
            MessageBox.Show(message);
        }
        public void ShowError(string message)
        {
            // 实现显示错误消息的逻辑
            Debug.WriteLine($"Error: {message}");
        }
        public void ShowWarning(string message)
        {
            // 实现显示警告消息的逻辑
            Debug.WriteLine($"Warning: {message}");
        }
    }
}
