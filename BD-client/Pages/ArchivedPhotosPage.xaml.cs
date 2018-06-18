using BD_client.ViewModels;
using BD_client.Windows;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BD_client.Dto;

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
            int selectedIndex = MyPhotosListBox.SelectedIndex;
            ViewModel.Preview(selectedIndex);
        }

        private void OnRestorePhoto(object sender, RoutedEventArgs e)
        {
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();
            ViewModel.Restore(photos);
        }

        private void OnDownloadPhoto(object sender, RoutedEventArgs e)
        {
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();
        }

        private void OnRemovePhoto(object sender, RoutedEventArgs e)
        {
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();
            ViewModel.Remove(photos);
        }
    }
}