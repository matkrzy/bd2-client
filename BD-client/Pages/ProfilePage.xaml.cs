using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for MyPhotos.xaml
    /// </summary>
    public partial class ProfilePage : Page
    { 
        ProfilePageViewModel vm = new ProfilePageViewModel(DialogCoordinator.Instance);

    public ProfilePage()
    {
        InitializeComponent();
        DataContext = vm;
    }
    }
}
