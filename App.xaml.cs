using System.Configuration;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Windows;
using AIMuster.Config;
using AIMuster.Utils;
using AIMuster.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AIMuster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static Mutex _mutex;
        public static IHost AppHost { get; private set; }
        public static AppConfig AppConfig { get; private set; }

        public App()
        {
            //AppHost = Host.CreateDefaultBuilder()
            //    .ConfigureServices((context, services) =>
            //    {
            //        services.AddSingleton<MainWindowViewModel>();
            //        //services.AddSingleton<IMessageService, MessageService>();
            //    })
            //    .Build();

            var asm = Assembly.GetExecutingAssembly();

            AppConfig = ConfigManager.LoadAppConfig();

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(provider =>
                    {
                        //立即保存回去一次（可选）
                        //ConfigManager.Save(config);
                        return AppConfig;
                    });
                    services.AddServicesByInterface(asm, "AIMuster.Services");
                    services.AddViewModels(asm, "AIMuster.ViewModels");
                })
                .Build();


            //LanguageManager.ChangeLanguage(config.Language);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {

            const string mutexName = "AIMuster";

            bool isNewInstance;
            _mutex = new Mutex(true, mutexName, out isNewInstance);

            if (!isNewInstance)
            {
                // 已有实例在运行
                MessageBox.Show("程序已在运行中。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                Shutdown();
                return;
            }

            await AppHost.StartAsync();

            var mainWindow = new MainWindow
            {
                DataContext = AppHost.Services.GetRequiredService<MainWindowViewModel>()
            };
            mainWindow.Show();

            LanguageManager.ChangeLanguage(AppConfig.Language);
            ThemeManager.SwitchTheme(AppConfig.Theme.ToString());

            base.OnStartup(e);


            //写入配置
            var aiModels = ConfigManager.LoadAiModelConfig();
            ConfigManager.SaveAiModelConfig(aiModels);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            base.OnExit(e);
        }
    }

}
