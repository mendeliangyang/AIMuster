using System.Configuration;
using System.Data;
using System.Reflection;
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

        public static IHost AppHost { get; private set; }

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

            var config = ConfigManager.Load();

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton(provider =>
                    {
                        //立即保存回去一次（可选）
                        //ConfigManager.Save(config);
                        return config;
                    });
                    services.AddServicesByInterface(asm, "AIMuster.Services");
                    services.AddViewModels(asm, "AIMuster.ViewModels");
                })
                .Build();


            LanguageManager.ChangeLanguage(config.Language);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            var mainWindow = new MainWindow
            {
                DataContext = AppHost.Services.GetRequiredService<MainWindowViewModel>()
            };
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            base.OnExit(e);
        }
    }

}
