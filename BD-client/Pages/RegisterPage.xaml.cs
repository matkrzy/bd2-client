using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        RegisterPageViewModel vm = new RegisterPageViewModel(DialogCoordinator.Instance);
        public RegisterPage()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
