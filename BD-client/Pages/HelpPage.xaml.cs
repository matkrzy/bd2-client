using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Page
    {
        public HelpPage()
        {
            InitializeComponent();
            HelpPageViewModel vm = new HelpPageViewModel(DialogCoordinator.Instance);
            DataContext = vm;
        }
    }
}
