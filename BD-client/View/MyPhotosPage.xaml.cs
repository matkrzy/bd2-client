using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Domain;
using RestSharp;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for MyPhotos.xaml
    /// </summary>
    public partial class MyPhotosPage : Page
    {

        public MyPhotosPage()
        {
            InitializeComponent();
            MyPhotosPageViewModel vm = new MyPhotosPageViewModel(DialogCoordinator.Instance);
            DataContext = vm;
        }



        private void OnPhotoDbClick(object sender, MouseButtonEventArgs e)
        { 
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

            foreach (var item in this.MyPhotosListBox.SelectedItems)
            {
                Photo photo = item as Photo;
                Task.Run(async ()=>
                {
                    IRestResponse response =  await new Request($"/photos/{photo.Id}").DoDelete();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        this.MyPhotosListBox.Items.Remove(photo);
                        this.MyPhotosListBox.ItemsSource = null;
//                        this.MyPhotosListBox.ItemsSource = 
                    }
                });
            }

            
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
