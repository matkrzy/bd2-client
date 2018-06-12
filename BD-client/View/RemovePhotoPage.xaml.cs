using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
