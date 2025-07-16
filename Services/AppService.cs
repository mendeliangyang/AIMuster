
using System.Windows;

namespace AIMuster.Services
{
    public class AppService : IAppService
    {
        public void Shutdown()
        {
            Application.Current.Shutdown();
        }
    }
}
