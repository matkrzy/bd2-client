using BD_client.Common;
using BD_client.Data.Photos;
using BD_client.Domain;
using BD_client.Domain.Enums;
using BD_client.Services;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                Photos[SelectedPhoto].Rate++;
            }
            else
            {
                var resRemove = await PhotoService.RemoveRate(Photos[SelectedPhoto].Id);
                if(resRemove)
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Unliked", "You disliked this photo");
                    Photos[SelectedPhoto].Rate--;
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
