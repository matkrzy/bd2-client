﻿using BD_client.Data.Photos;
using BD_client.Domain;
using BD_client.Services;
using BD_client.ViewModels;
using BD_client.Windows;
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
    /// Interaction logic for MyPhotos.xaml
    /// </summary>
    public partial class MyPhotosPage : Page
    {
        public MyPhotosPageViewModel ViewModel;

        public MyPhotosPage()
        {
            InitializeComponent();
            //MyPhotosListBox.MouseDoubleClick += async (s, e) => OnPhotoDbClick(s, e);
            ViewModel = new MyPhotosPageViewModel(DialogCoordinator.Instance);
            DataContext = ViewModel;
        }



        private void OnPhotoDbClick(object sender, MouseButtonEventArgs e)
        {
            var allPhotos = ViewModel.Photos.Result;
            new PhotoDetailsWindow(allPhotos, MyPhotosListBox.SelectedIndex).Show();

        }

        private void OnArchivePhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add((MainWindow.MainVM.Photos[this.MyPhotosListBox.Items.IndexOf(item)].Id));
            }

            foreach (var id in list)
            {
                ViewModel.Archive(id);
            }
            ViewModel.Photos.Result.Update();
        }

        private void OnEditPhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add(this.MyPhotosListBox.Items.IndexOf(item));// Add selected indexes to the List<int>
            }
            MainWindow.MainVM.List = list;
            MainWindow.MainVM.SelectedIndex = 1;
            MainWindow.MainVM.Page = "Pages/EditPhotoPage.xaml";
        }
        private void OnDownloadPhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add(this.MyPhotosListBox.Items.IndexOf(item));// Add selected indexes to the List<int>
            }
            MainWindow.MainVM.List = list;
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
            MainWindow.MainVM.List = list;
            MainWindow.MainVM.SelectedIndex = 4;
            MainWindow.MainVM.Page = "Pages/RemovePhotoPage.xaml";
        }

        private void OnSharePhoto(object sender, RoutedEventArgs e)
        {
            List<int> list = new List<int>();

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                list.Add(this.MyPhotosListBox.Items.IndexOf(item));// Add selected indexes to the List<int>
            }
            MainWindow.MainVM.List = list;
            MainWindow.MainVM.SelectedIndex = 5;
            MainWindow.MainVM.Page = "Pages/SharePage.xaml";
        }
    }
}
