using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Windows.Input;
using BD_client.Models;

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
