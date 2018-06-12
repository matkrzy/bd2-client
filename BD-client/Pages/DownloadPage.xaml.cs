using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : Page
    {

        DownloadPageViewModel vm = new DownloadPageViewModel(DialogCoordinator.Instance);
        public DownloadPage()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
