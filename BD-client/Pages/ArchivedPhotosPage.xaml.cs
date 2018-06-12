using BD_client.ViewModels;
using BD_client.Windows;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for MyPhotos.xaml
    /// </summary>
    public partial class ArchivedPhotosPage : Page
    {
        public ArchivedPhotosPageViewModel ViewModel;

        public ArchivedPhotosPage()
        {
            InitializeComponent();
            ViewModel = new ArchivedPhotosPageViewModel(DialogCoordinator.Instance);
            DataContext = ViewModel;
        }



        private void OnPhotoDbClick(object sender, MouseButtonEventArgs e)
        {
            var allPhotos = ViewModel.Photos.Result;
            new PhotoDetailsWindow(allPhotos, MyPhotosListBox.SelectedIndex).Show();
        }

        private void OnReactivePhoto(object sender, RoutedEventArgs e)
        {
            List<long> list = new List<long>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add((MainWindow.MainVM.Photos[this.MyPhotosListBox.Items.IndexOf(item)].Id));
            }

            foreach(var id in list)
            {
                ViewModel.Reactive(id);
            }
            ViewModel.Photos.Result.Update();
        }

        private void OnDownloadPhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add(this.MyPhotosListBox.Items.IndexOf(item));// Add selected indexes to the List<int>
            }
//            MainWindow.MainVM.List = list;
            MainWindow.MainVM.SelectedIndex = 3;
            MainWindow.MainVM.Page = "Pages/DownloadPage.xaml";
        }
        private void OnRemovePhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add(this.MyPhotosListBox.Items.IndexOf(item));// Add selected indexes to the List<int>
            }
//            MainWindow.MainVM.List = list;
            MainWindow.MainVM.SelectedIndex = 4;
            MainWindow.MainVM.Page = "Pages/RemovePhotoPage.xaml";
        }
    }
}
