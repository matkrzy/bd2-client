using System;
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
            String page = "";

            switch (this.HamburgerMenuControl.SelectedIndex)
            {
                case 0:
                    page = "AddPhotosPage.xaml";
                    break;
                case 1:
                    page = "EditPhotoPage.xaml";
                    break;
                case 2:
                    page = "SearchPage.xaml";
                    break;
                case 3:
                    page = "DownloadPage.xaml";
                    break;
                case 4:
                    page = "RemovePhotoPage.xaml";
                    break;
                case 5:
                    page = "SharePage.xaml";
                    break;
            }

            MainVM.Page = $"View/{page}";

        }
    }
}