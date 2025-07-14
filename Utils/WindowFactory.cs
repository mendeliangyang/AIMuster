using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace AIMuster.Utils
{
    

    public enum ShowMode
    {
        None,
        Show,
        ShowDialog
    }

    public static class WindowFactory
    {
        public static T ShowWindow<T>(ShowMode showMode=ShowMode.Show) where T : Window, new()
        {

            var window = new T();
            var windowType = window.GetType();
            var ns = windowType.Namespace;

            // 假设 ViewModel 命名空间为 AIMuster.ViewModels，类型名为 XxxWindowViewModel
            // 你可以根据实际情况调整命名空间拼接逻辑
            var vmTypeName = $"{ns?.Replace("UI", "ViewModels")}.{windowType.Name.Replace("Window", "WindowViewModel")}";
            var asm = windowType.Assembly;
            var vmType = asm.GetType(vmTypeName);

            if (vmType != null)
            {
                var vm = App.AppHost.Services.GetService(vmType);
                window.DataContext = vm;
            }
            switch (showMode)
            {
                case ShowMode.Show:
                    window.Show();
                    break;
                case ShowMode.ShowDialog:
                    window.ShowDialog();
                    break;
                case ShowMode.None:
                    // 不显示窗口
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(showMode), showMode, null);
            }
            return window;
        }
    }


    public static class ServiceCollectionExtensions
    {
        public static void AddViewModels(this IServiceCollection services, Assembly assembly, string @namespace)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == @namespace);

            foreach (var type in types)
            {
                services.AddSingleton(type);
            }
        }

        public static void AddServicesByInterface(this IServiceCollection services, Assembly assembly, string @namespace)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == @namespace);

            foreach (var impl in types)
            {
                var iface = impl.GetInterfaces().FirstOrDefault(i => i.Namespace == @namespace && i.Name == $"I{impl.Name}");
                if (iface != null)
                {
                    services.AddSingleton(iface, impl);
                }
            }
        }
    }


    public static class LanguageManager
    {
        public static void ChangeLanguage(string language)
        {
            var dict = new ResourceDictionary();
            switch (language)
            {
                case "zh":
                    dict.Source = new Uri("Resources/Strings.zh.xaml", UriKind.Relative);
                    break;
                case "en":
                    dict.Source = new Uri("Resources/Strings.en.xaml", UriKind.Relative);
                    break;
                default:
                    throw new NotSupportedException("Language not supported");
            }

            // 替换语言资源字典
            var oldDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Strings."));

            if (oldDict != null)
                Application.Current.Resources.MergedDictionaries.Remove(oldDict);

            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
    public static class ThemeManager
    {
        public static void SwitchTheme(string theme)
        {
            string themePath = $"Resources/Themes/{theme}Theme.xaml";

            var newDict = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Relative)
            };

            // 移除旧主题
            var appResources = Application.Current.Resources.MergedDictionaries;
            var existingTheme = appResources.FirstOrDefault(d => d.Source?.OriginalString.Contains("Theme") == true);
            if (existingTheme != null)
                appResources.Remove(existingTheme);

            // 加载新主题
            appResources.Add(newDict);
        }
    }

}
