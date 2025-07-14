using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AIMuster.Models;
using Microsoft.Web.WebView2.Wpf;

namespace AIMuster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WebViewer_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is WebView2 webView && webView.DataContext is AiModelConfig model)
            {
                model.TargetWebView = webView;
            }
        }
    }
}