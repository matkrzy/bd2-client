using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BD_client.Dto;
using BD_client.Models;
using BD_client.ViewModels;

namespace BD_client.Windows
{
    /// <summary>
    /// Interaction logic for PhotoDetailsWindow.xaml
    /// </summary>
    public partial class PhotoDetailsWindow : Window
    {
        private int CurrentItemIndex;
        private int PhotoCollectionCount;
        private ObservableCollection<Photo> photos;

        public PhotoDetailsWindow(ObservableCollection<Photo> photos, int selectedPhotoIndex)
        {
            InitializeComponent();
       
            this.photos = photos;
            CurrentItemIndex = selectedPhotoIndex;
            PhotoCollectionCount = photos.Count;

            KeyDown += OnKeyDown;

            var photo = photos.ElementAt(CurrentItemIndex);
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
     
            DataContext = new PhotoDetailsWindowViewModel(photos.ElementAt(CurrentItemIndex));
        }

    }
}
