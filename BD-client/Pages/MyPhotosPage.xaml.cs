using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Enums;
using BD_client.Windows;
using RestSharp;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for MyPhotos.xaml
    /// </summary>
    public partial class MyPhotosPage : Page
    {
        public MyPhotosPageViewModel ViewModel = new MyPhotosPageViewModel(DialogCoordinator.Instance);

        public MyPhotosPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }


        private void OnPhotoDbClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = MyPhotosListBox.SelectedIndex;
            ViewModel.Preview(selectedIndex);
        }

        private void OnArchivePhoto(object sender, RoutedEventArgs e)
        {
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();

            ViewModel.ArchivePhoto(photos);
        }

        private void OnEditPhoto(object sender, RoutedEventArgs e)
        {
            MainWindow.MainVM.Photos = new List<Photo>();

            foreach (Photo photo in this.MyPhotosListBox.SelectedItems)
            {
                MainWindow.MainVM.Photos.Add(photo);
            }


            MainWindow.MainVM.SelectedIndex = 1;
            MainWindow.MainVM.Page = "EditPhotoPage.xaml";
        }

        private void OnDownloadPhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add(this.MyPhotosListBox.Items.IndexOf(item)); // Add selected indexes to the List<int>
            }

//            MainWindow.MainVM.List = list;
            MainWindow.MainVM.SelectedIndex = 3;
            MainWindow.MainVM.Page = "DownloadPage.xaml";
        }

        private async void OnRemovePhoto(object sender, RoutedEventArgs e)
        {
            ViewModel.GetAllUserPhotos();
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();

            ViewModel.RemovePhotos(photos);
        }

        private void OnSharePhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add(this.MyPhotosListBox.Items.IndexOf(item)); // Add selected indexes to the List<int>
            }

//            MainWindow.MainVM.List = list;
            MainWindow.MainVM.SelectedIndex = 5;
            MainWindow.MainVM.Page = "SharePage.xaml";
        }
    }
}