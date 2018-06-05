using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for RemovePhotoPage.xaml
    /// </summary>
    public partial class RemovePhotoPage : Page
    {
        RemovePhotoPageViewModel vm = new RemovePhotoPageViewModel(DialogCoordinator.Instance);
        public RemovePhotoPage()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
