using System;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BD_client.Models;
using BD_client.Services;

namespace BD_client.ViewModels
{
    public class PublicPhotosPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        public PhotoCollection Photos { get; set; }
        public ICommand LikeCmd { get; set; }
        private IDialogCoordinator dialogCoordinator;
        private int _selectedPhoto;

        public int SelectedPhoto
        {
            get
            {
                return _selectedPhoto;
            }
            set
            {
                _selectedPhoto = value;
                OnPropertyChanged("SelectedPhoto");
            }
        }

        public PublicPhotosPageViewModel(IDialogCoordinator instance, string photoDestination)
        {
            try
            {
                Photos = new PhotoCollection(photoDestination);
            }
            catch (Exception) { }
            dialogCoordinator = instance;
            LikeCmd = new RelayCommand(async x => await Like());
        }

        private async Task Like()
        {
            var resAdd = await PhotoService.AddRate(Photos[SelectedPhoto].Id);
            if (resAdd)
            {
                await dialogCoordinator.ShowMessageAsync(this, "New like", "You ♥ this photo");
//                Photos[SelectedPhoto].Rate++;
            }
            else
            {
                var resRemove = await PhotoService.RemoveRate(Photos[SelectedPhoto].Id);
                if(resRemove)
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Unliked", "You disliked this photo");
//                    Photos[SelectedPhoto].Rate--;
                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", "Error occured");
                }
            }
            Photos.Update();
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
