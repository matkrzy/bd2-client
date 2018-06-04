using BD_client.Domain;
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
using System.Windows.Shapes;

namespace BD_client.Windows
{
    /// <summary>
    /// Interaction logic for SelectPhotosModalWindow.xaml
    /// </summary>
    public partial class SelectPhotosModalWindow : Window
    {
        private PhotoCollection Photos { get; set; }

        public SelectPhotosModalWindow(IEnumerable<int> alreadyAssignedPhotosIds)
        {
            InitializeComponent();
            var path = System.IO.Directory.GetCurrentDirectory() + @"\..\..\tmp\own";
            Photos = new PhotoCollection(path);
            Photos.Update(alreadyAssignedPhotosIds);
            DataContext = Photos;      
        }

        private void OnAddSelectedPhotosClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        //TODO
        public IEnumerable<int> SelectedPhotosIds
        {
            get { return PhotosListBox.SelectedItems.Cast<Photo>().Select(x => x.Id); }
        }
    }
}
