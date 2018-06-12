using BD_client.ViewModels.Windows;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BD_client.Models;

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
