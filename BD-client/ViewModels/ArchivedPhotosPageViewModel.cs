using BD_client.Common;
using BD_client.Services;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using BD_client.Enums;
using BD_client.Models;
using BD_client.Pages;

namespace BD_client.ViewModels
{
    public class ArchivedPhotosPageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged = null;

        private string _page;
        public NotifyTaskCompletion<PhotoCollection> Photos { get; set; }

        private IDialogCoordinator dialogCoordinator;


        public string Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public ArchivedPhotosPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "//Img//photos";
            Photos = new NotifyTaskCompletion<PhotoCollection>(GetArchivedUserPhotos());
        }


        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async Task<PhotoCollection> GetArchivedUserPhotos()
        {
            var destination = Directory.GetCurrentDirectory() + @"\..\..\tmp\own";
            var photos = await PhotoService.GetArchivedUserPhotos();
            MainWindow.MainVM.Photos = photos;
            //TODO: różne typy zdjęć, nie tylko jpg
            foreach (var photo in photos)
            {
                var completePath = $@"{destination}\{photo.Id}.jpg";
                if (!File.Exists(completePath))
                {
                    // jeżeli zdjęcie nie jest jeszcze pobrane
                    if (!(await ImageService.DownloadImageToLocation(completePath, photo.Id)))
                    {
                        //TODO: wyświetlić komunikat informujący o błędzie
                    }
                }
            }
            return new PhotoCollection(destination, photos);
        }

        public async void Reactive(long id)
        {
            var res = await PhotoService.ChangePhotoState(PhotoState.ACTIVE, id);
            foreach(var photo in MainWindow.MainVM.Photos)
            {
                if (photo.Id == id)
                {
                    Photos.Result.Remove(photo);
                    MainWindow.MainVM.Photos.Remove(photo);
                    break;
                }
            }
            
            
        }



    }
}
