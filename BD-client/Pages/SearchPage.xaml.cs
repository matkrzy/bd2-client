using BD_client.Dto;
using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        SearchPageViewModel vm = new SearchPageViewModel(DialogCoordinator.Instance);
        MyPhotosPageViewModel MyPhotosVM = new MyPhotosPageViewModel(DialogCoordinator.Instance);

        public SearchPage()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void OnPhotoDbClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = MyPhotosListBox.SelectedIndex;
            MyPhotosVM.Preview(selectedIndex);
        }

        private void OnArchivePhoto(object sender, RoutedEventArgs e)
        {
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();

            MyPhotosVM.ArchivePhoto(photos);
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
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();
            MyPhotosVM.Download(photos);
        }

        private async void OnRemovePhoto(object sender, RoutedEventArgs e)
        {
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();

            MyPhotosVM.RemovePhotos(photos);
        }

        private void OnSharePhoto(object sender, RoutedEventArgs e)
        {
            List<Photo> photos = this.MyPhotosListBox.SelectedItems.OfType<Photo>().ToList();

            MyPhotosVM.ShareDialog(photos);
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            bool found = false;
            var border = (resultStack.Parent as ScrollViewer).Parent as Border;
            var data = vm.TagsAutocomplete;

            string query = (sender as TextBox).Text;

            if (query.Length == 0)
            {
                // Clear
                resultStack.Children.Clear();
                border.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                border.Visibility = System.Windows.Visibility.Visible;
            }

            // Clear the list
            resultStack.Children.Clear();

            // Add the result
            foreach (var obj in data)
            {
                // The word starts with this... Autocomplete must work
                addItem(obj);
                found = true;
                
            }

            if (!found)
            {
                resultStack.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }

        private void addItem(string text)
        {
            TextBlock block = new TextBlock();

            // Add the text
            block.Text = text;

            // A little style...
            block.Margin = new Thickness(2, 3, 2, 3);
            block.Cursor = Cursors.Hand;

            // Mouse events
            block.MouseLeftButtonUp += (sender, e) =>
            {
                searchTags.Text = (sender as TextBlock).Text;
            };

            block.MouseEnter += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.PeachPuff;
            };

            block.MouseLeave += (sender, e) =>
            {
                TextBlock b = sender as TextBlock;
                b.Background = Brushes.Transparent;
            };

            // Add to the panel
            resultStack.Children.Add(block);
        }

    }
}
