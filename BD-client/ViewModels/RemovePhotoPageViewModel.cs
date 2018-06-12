using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using BD_client.Dto;
using BD_client.Pages;
using BD_client.Services;

namespace BD_client.ViewModels
{
    class RemovePhotoPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        public ObservableCollection<Photo> Photos { get; set; }

        public ICommand CancelCmd { get; set; }
        public ICommand RemoveCmd { get; set; }
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


        public RemovePhotoPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            Photos = new ObservableCollection<Photo>();
            RemoveCmd = new RelayCommand(x => Remove());
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

        private async void Remove()
        {
            List<int> photoIndex = RemovePhotoFromServer();
            await dialogCoordinator.ShowMessageAsync(this, "Result", photoIndex.Count + " of " + Photos.Count + " photos was removed");
            for (int i = photoIndex.Count - 1; i >= 0; i--)
            {
                Photos.RemoveAt(photoIndex[i]);
            }
        }

        private void RemovePhotoFromDisc(long name)
        {
            var destination = System.IO.Directory.GetCurrentDirectory() + @"\..\..\tmp\own\"+name+".jpg";
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            else
                throw new Exception();
        }

        private List<int> RemovePhotoFromServer()
        {
            var photoIndex = new List<int>();
            for (int i = 0; i < Photos.Count; i++)
            {
                var photosUrl = MainWindow.MainVM.BaseUrl + "api/v1/photos/"+Photos[i].Id;
                try
                {
                    RemovePhotoFromDisc(Photos[i].Id);
                    ApiRequest.Delete(photosUrl);
                    photoIndex.Add(i);
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                }
            }
            return photoIndex;

        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }


        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
