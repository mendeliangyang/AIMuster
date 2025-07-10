using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMuster.Services
{
    public interface IMessageService
    {
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message">消息内容</param>
        void ShowMessage(string message);
        /// <summary>
        /// 显示错误消息
        /// </summary>
        /// <param name="message">错误内容</param>
        void ShowError(string message);
        /// <summary>
        /// 显示警告消息
        /// </summary>
        /// <param name="message">警告内容</param>
        void ShowWarning(string message);
    }
}
