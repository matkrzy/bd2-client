using BD_client.ViewModels;
using MahApps.Metro.Controls;
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
    /// Interaction logic for AddPhotosPage.xaml
    /// </summary>
    public partial class AddPhotosPage : Page
    {

        AddPhotosPageViewModel vm = new AddPhotosPageViewModel(DialogCoordinator.Instance);
        public AddPhotosPage()
        {
            InitializeComponent();
            DataContext = vm;
        }

        public void HamburgerMenuControl_OnItemClick(object sender, ItemClickEventArgs e)
        {
            //// set the content
            //this.HamburgerMenuControl.Content = e.ClickedItem;
            //// close the pane
            //this.HamburgerMenuControl.IsPaneOpen = false;

            //if (this.HamburgerMenuControl.SelectedIndex == 0)
            //    MyPhotosPageViewModel.ShowAddPhotos();

            //else if (this.HamburgerMenuControl.SelectedIndex == 1)
            //    MyPhotosPageViewModel.EditPhoto();

            //else if (this.HamburgerMenuControl.SelectedIndex == 2)
            //    MyPhotosPageViewModel.Search();

            //else if (this.HamburgerMenuControl.SelectedIndex == 3)
            //    MyPhotosPageViewModel.Download();

            //else if (this.HamburgerMenuControl.SelectedIndex == 4)
            //    MyPhotosPageViewModel.RemovePhoto();

            //else if (this.HamburgerMenuControl.SelectedIndex == 5)
            //    MyPhotosPageViewModel.SharePhoto();

        }

    }
}
