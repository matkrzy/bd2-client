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
using MahApps.Metro.Controls;
using BD_client.ViewModels;

namespace BD_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindowViewModel MainVM { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainVM = new MainWindowViewModel();
            DataContext = MainVM;
        }
        public void HamburgerMenuControl_OnItemClick(object sender, ItemClickEventArgs e)
        {

            // close the pane
            this.HamburgerMenuControl.IsPaneOpen = false;

            if (this.HamburgerMenuControl.SelectedIndex == 0)
                MainWindow.MainVM.Page = "Pages/AddPhotosPage.xaml";

            else if (this.HamburgerMenuControl.SelectedIndex == 1)
                MainWindow.MainVM.Page = "Pages/EditPhotoPage.xaml";

            else if (this.HamburgerMenuControl.SelectedIndex == 2)
                MainWindow.MainVM.Page = "Pages/SearchPage.xaml";

            else if (this.HamburgerMenuControl.SelectedIndex == 3)
                MainWindow.MainVM.Page = "Pages/DownloadPage.xaml";

            else if (this.HamburgerMenuControl.SelectedIndex == 4)
                MainWindow.MainVM.Page = "Pages/RemovePhotoPage.xaml";

            else if (this.HamburgerMenuControl.SelectedIndex == 5)
                MainWindow.MainVM.Page = "Pages/SharePage.xaml";

        }
        





    }
}