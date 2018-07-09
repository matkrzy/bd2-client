using System.Windows;
using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for EditPhotoPage.xaml
    /// </summary>
    public partial class EditPhotoPage : Page
    {
        EditPhotoPageViewModel vm = new EditPhotoPageViewModel(DialogCoordinator.Instance);

        public EditPhotoPage()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void Description_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.description.Width = this.DescCol.ActualWidth - 10;
            this.DescGrid.Height = this.DescRow.ActualHeight;
        }

        private void Tags_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.tags.Width = TagsCol.ActualWidth - 10;
            this.DescGrid.Height = this.DescRow.ActualHeight;
        }

        private void EditAllCheckbox_OnChange(object sender, RoutedEventArgs e)
        {

            foreach (var column in this.PhotosDataGrid.Columns)
            {
                if (column.Header.Equals("Description") || column.Header.Equals("Tags"))
                {
                    bool checkboxState = (bool) EditAllCheckbox.IsChecked;
                    column.Visibility = checkboxState ? Visibility.Hidden : Visibility.Visible;
                    description.IsEnabled = checkboxState;
                    tags.IsEnabled = checkboxState;
                }
            }
        }

        private void PhotosDataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Description")
            {
                var col = e.Column as DataGridTextColumn;

                var style = new Style(typeof(TextBlock));
                style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));
                style.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));

                col.ElementStyle = style;
            }
        }
    }
}