using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for SharePage.xaml
    /// </summary>
    public partial class SharePage : Page
    {
        SharePageViewModel vm = new SharePageViewModel(DialogCoordinator.Instance);
        public SharePage()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
