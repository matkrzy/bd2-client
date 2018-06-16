using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Net;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Pages;
using RestSharp;

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
            openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                Multiselect = true
            };
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
                    Photos.Add(new Photo()
                    {
                        Path = openFileDialog.FileNames[i],
                        Name = openFileDialog.SafeFileNames[i]
                    });
                }
            }
        }

        private async void Add()
        {
            bool errorOccurred = false;
            var progressBar = await dialogCoordinator.ShowProgressAsync(this, "Uploading", "Starting uploading");
            List<string> failedPhotos = new List<string>();

            for (int i = 0; i < Photos.Count; i++)
            {
                Photo photo = Photos[i];

                progressBar.SetTitle($"Adding {i + 1} of {Photos.Count}");
                progressBar.SetMessage($"Adding {photo.Name}");
                progressBar.SetProgress((double) (i + 1) / Photos.Count);

                Request request = new Request("/photos");
                request.AddFile(photo.Path);
                request.AddParameter("description", photo.Description);
                request.AddParameter("name", photo.Name);
                IRestResponse response = await request.DoPost();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    errorOccurred = true;
                    failedPhotos.Add(photo.Name);
                }
            }

            await progressBar.CloseAsync();


            if (errorOccurred)
            {
                foreach (string name in failedPhotos)
                {
                    Photos.Remove(Photos.Single(i => i.Name == name));
                }

                await dialogCoordinator.ShowMessageAsync(this, "Oooppss", "Something went wrong. Try again!");
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos added");
                Photos.Clear();

                MainWindow.MainVM.Page = "MyPhotosPage.xaml";
                MainWindow.MainVM.SelectedIndex = -1;
            }
        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        public string Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public int DataGridSelectedIndex
        {
            get { return _dataGridSelectedIndex; }
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