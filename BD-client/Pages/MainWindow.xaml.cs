using System;
using BD_client.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BD_client.Pages
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
            MainVM = new MainWindowViewModel(DialogCoordinator.Instance);
            DataContext = MainVM;
        }

        public void HamburgerMenuControl_OnItemClick(object sender, ItemClickEventArgs e)
        {
            // close the pane
            this.HamburgerMenuControl.IsPaneOpen = false;

            String page = "";
            switch (this.HamburgerMenuControl.SelectedIndex)
            {
                case 0:
                    page = "AddPhotosView.xaml";
                    break;
                case 1:
                    page = "EditPhotoPage.xaml";
                    break;
                case 2:
                    page = "SearchPage.xaml";
                    break;
            }

            MainWindow.MainVM.Page = page;
        }
    }
}