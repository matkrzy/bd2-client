using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for EditPhotoPage.xaml
    /// </summary>
    public partial class EditPhotoPage : Page
    {
        EditPhotoPageViewModel vm = new EditPhotoPageViewModel(DialogCoordinator.Instance);
        public EditPhotoPage()
        {
            InitializeComponent();
            DataContext = vm;
        }

    }
}
