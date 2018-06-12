using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        LogInPageViewModel vm = new LogInPageViewModel(DialogCoordinator.Instance);
        public LogInPage()
        { 
            InitializeComponent();
            DataContext = vm;
        }
    }
}
