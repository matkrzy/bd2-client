using BD_client.Domain;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using BD_client.Services;

namespace BD_client.ViewModels
{
    class DownloadPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        public ObservableCollection<Photo> Photos { get; set; }

        public ICommand CancelCmd { get; set; }
        public ICommand DownloadCmd { get; set; }
        public ICommand RemovePhotoCmd { get; set; }
        private IDialogCoordinator dialogCoordinator;
        private int _dataGridSelectedIndex;
        private string _page;
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


        public DownloadPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            Photos = new ObservableCollection<Photo>();
            DownloadCmd = new RelayCommand(x => Download());
            CancelCmd = new RelayCommand(x => Cancel());
            RemovePhotoCmd = new RelayCommand(x => RemovePhoto());
            if (MainWindow.MainVM.List != null)
                GetSelectedPhtotos();
        }

        private void GetSelectedPhtotos()
        {
            for (int i = 0; i < MainWindow.MainVM.List.Count; i++)
            {
                int index = MainWindow.MainVM.List[i];
                Photo newPhoto = MainWindow.MainVM.Photos[index];
                Photos.Add(newPhoto);
            }
            MainWindow.MainVM.List.Clear();
            MainWindow.MainVM.List = null;
        }

        private void RemovePhoto()
        {
            Photos.RemoveAt(DataGridSelectedIndex);
        }

        private async void Download()
        {
            using (FolderBrowserDialog browser = new FolderBrowserDialog())
            {
                browser.Description = "Select a folder";
                if (browser.ShowDialog() == DialogResult.OK)
                {
                    List<int> photoIndex = await DownloadPhotos(browser.SelectedPath);
                    await dialogCoordinator.ShowMessageAsync(this, "Result", Photos.Count + " of " + Photos.Count + " photos was downloaded");
                    for (int i = photoIndex.Count - 1; i >= 0; i--)
                    {
                        Photos.RemoveAt(photoIndex[i]);
                    }
                }
            }
        }

        private async Task<List<int>> DownloadPhotos(string path)
        {
            var photoIndex = new List<int>();
            for (int i = 0; i < Photos.Count; i++)
            {
                string imagePath = path +"\\"+ Photos[i].Id+".jpg";
                if(await ImageService.DownloadImageToLocation(imagePath, Photos[i].Id))
                    photoIndex.Add(i);
            }
            return photoIndex;
        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }


        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
