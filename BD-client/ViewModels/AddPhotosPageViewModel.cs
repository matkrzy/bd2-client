using BD_client.Domain;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;
using BD_client.Services;

namespace BD_client.ViewModels
{
    class AddPhotosPageViewModel : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged = null;
        public ObservableCollection<Photo> Photos { get; set; }

        public ICommand BrowseCmd { get; set; }
        public ICommand AddCmd { get; set; }
        public ICommand CancelCmd { get; set; }
        public ICommand RemovePhotoCmd { get; set; }
        private OpenFileDialog openFileDialog;
        private IDialogCoordinator dialogCoordinator;
        private int _dataGridSelectedIndex;
        private String _page;


        public AddPhotosPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            BrowseCmd = new RelayCommand(x => Browse());
            CancelCmd = new RelayCommand(x => Cancel());
            RemovePhotoCmd = new RelayCommand(x => RemovePhoto());
            AddCmd = new RelayCommand(x => Add());
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.Multiselect = true;
            Photos = new ObservableCollection<Photo>();

        }

        private void RemovePhoto()
        {
            Photos.RemoveAt(DataGridSelectedIndex);
        }

        private void Browse()
        {

            var result = openFileDialog.ShowDialog();

            if (result == true)
            {

                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    Photos.Add(new Domain.Photo() { Id = i + 1, Path = openFileDialog.FileNames[i], Name = openFileDialog.SafeFileNames[i], UploadTime = DateTime.Now });
                }
            }
        }

        private async Task<List<int>> AddPhotos()
        {
            var photoIndex = new List<int>();
            for (int i = 0; i < Photos.Count; i++)
            {
                //TODO: co z photoState i shareState ?
                //var addedPhotoId = await PhotoService.AddPhoto(Photos[i].Name, Photos[i].Description, Photos[i].PhotoState, Photos[i].ShareState);
                var addedPhotoId = await PhotoService.AddPhoto(Photos[i].Name, Photos[i].Description, PhotoState.ACTIVE, ShareState.PRIVATE);
                if (await ImageService.UploadImage(addedPhotoId, Photos[i].Path, Photos[i].Name))
                {
                    photoIndex.Add(i);
                }
            }
            return photoIndex;
        }
        private async void Add()
        {

            List<int> photoIndex = await AddPhotos();
            await dialogCoordinator.ShowMessageAsync(this, "Result", photoIndex.Count + " of " + Photos.Count + " photos was added");
            for (int i = photoIndex.Count - 1; i >= 0; i--)
            {
                Photos.RemoveAt(photoIndex[i]);
            }
        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

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

        public int DataGridSelectedIndex
        {
            get
            {
                return _dataGridSelectedIndex;
            }
            set
            {
                _dataGridSelectedIndex = value;
                OnPropertyChanged("DataGridSelectedIndex");
            }
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
