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

        public PublicPhotosPageViewModel(IDialogCoordinator instance, string photoDestination)
        {
            Photos = new PhotoCollection(photoDestination);
            dialogCoordinator = instance;
            LikeCmd = new RelayCommand(x => Like());
        }

        private async void Like()
        {
            await dialogCoordinator.ShowMessageAsync(this, "New like", "You ♥ this photo");
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
