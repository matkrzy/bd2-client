using BD_client.Domain;
using BD_client.ViewModels.Windows;
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
    /// Interaction logic for PhotoDetailsWindow.xaml
    /// </summary>
    public partial class PhotoDetailsWindow : Window
    {
        public PhotoCollection Photos { get; set; }

        private int CurrentItemIndex;
        private int PhotoCollectionCount;

        public PhotoDetailsWindow(PhotoCollection photos, int selectedPhotoIndex)
        {
            InitializeComponent();
            Photos = photos;

            CurrentItemIndex = selectedPhotoIndex;
            PhotoCollectionCount = Photos.Count;

            KeyDown += OnKeyDown;

            var photo = Photos.ElementAt(CurrentItemIndex);
            DataContext = new PhotoDetailsWindowViewModel(photo);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    return;
                case Key.Right:
                    CurrentItemIndex++;
                    break;
                case Key.Left:
                    CurrentItemIndex--;
                    break;
            }
            UpdateViewModel();
        }

        private void UpdateViewModel()
        {
            if (CurrentItemIndex >= PhotoCollectionCount)
            {
                CurrentItemIndex = 0;
            }
            else if(CurrentItemIndex < 0)
            {
                CurrentItemIndex = PhotoCollectionCount - 1;
            }
            var newPhoto = Photos.ElementAt(CurrentItemIndex);
            DataContext = new PhotoDetailsWindowViewModel(newPhoto);
        }

    }
}
