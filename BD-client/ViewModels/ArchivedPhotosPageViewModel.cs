using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BD_client.Common;
using BD_client.Services;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Enums;
using BD_client.Models;
using BD_client.Pages;
using BD_client.Windows;
using Newtonsoft.Json;
using RestSharp;

namespace BD_client.ViewModels
{
    public class ArchivedPhotosPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        private string _page;
        public ObservableCollection<Photo> _Photos { get; set; }

        public ObservableCollection<Photo> Photos
        {
            get { return _Photos; }
            set
            {
                _Photos = value;
                OnPropertyChanged("Photos");
            }
        }

        private IDialogCoordinator dialogCoordinator;


        public string Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public ArchivedPhotosPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            GetArchivedUserPhotos();
        }


        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async void GetArchivedUserPhotos(PhotoState state = PhotoState.ARCHIVED)
        {
            String userId = ConfigurationManager.AppSettings["Id"];
            Request request = new Request($"/users/{userId}/photos");
            request.AddParameter("state", state.ToString());

            IRestResponse response = await request.DoGet();

            Photos = JsonConvert.DeserializeObject<ObservableCollection<Photo>>(response.Content);
        }

        public async void Restore(List<Photo> photos)
        {
            bool errorOccurred = false;
            var progressBar = await dialogCoordinator.ShowProgressAsync(this, "Restoring", "Starting restoring");

            for (int i = 0; i < photos.Count; i++)
            {
                Photo photo = photos[i];

                progressBar.SetTitle($"Restoring {i + 1} of {photos.Count}");
                progressBar.SetMessage($"Restoring {photo.Name}");
                progressBar.SetProgress((double) (i + 1) / photos.Count);

                photo.PhotoState = PhotoState.ACTIVE;
                IRestResponse response = await new Request($"/photos/{photo.Id}").DoPut(photo);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    errorOccurred = true;
                }
            }

            await progressBar.CloseAsync();


            if (errorOccurred)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Oooppss",
                    "Something went wrong. Try again!");
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos restored");

                GetArchivedUserPhotos();
            }
        }

        public async void Remove(List<Photo> photos)
        {
            var confirm =
                await dialogCoordinator.ShowMessageAsync(this, "Are you sure?",
                    $"Are you sure that to delete {photos.Count}", MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "OK",
                        NegativeButtonText = "CANCEL",
                        AnimateHide = true,
                        AnimateShow = true,
                    });

            if (confirm == MessageDialogResult.Negative)
            {
                return;
            }

            bool errorOccurred = false;
            var progressBar = await dialogCoordinator.ShowProgressAsync(this, "Deleting", "Starting deleting");


            for (int i = 0; i < photos.Count; i++)
            {
                Photo photo = photos[i];

                progressBar.SetTitle($"Deleting {i + 1} of {photos.Count}");
                progressBar.SetMessage($"Deleting {photo.Name}");
                progressBar.SetProgress((double) (i + 1) / photos.Count);

                IRestResponse response = await new Request($"/photos/{photo.Id}").DoDelete();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    errorOccurred = true;
                }
            }

            await progressBar.CloseAsync();

            if (errorOccurred)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Oooppss",
                    "Something went wrong. Try again!");
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos deleted");

                GetArchivedUserPhotos();
            }
        }

        public async void Download(List<Photo> photos)
        {
            bool errorOccurred = false;

            var dialog = new FolderBrowserDialog();
            var progressBar =
                await dialogCoordinator.ShowProgressAsync(this, "Downloading", "Starting downloading");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < photos.Count; i++)
                {
                    Photo photo = photos[i];
                    bool status = await new Request(photo.Url).Download(dialog.SelectedPath, photo.Name,
                        Path.GetExtension(photo.Path));


                    progressBar.SetTitle($"Downloading {i + 1} of {photos.Count}");
                    progressBar.SetMessage($"Downloading {photo.Name}");
                    progressBar.SetProgress((double)(i + 1) / photos.Count);

                    if (!status)
                    {
                        errorOccurred = true;
                    }
                }

                await progressBar.CloseAsync();

                if (errorOccurred)
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Oooppss",
                        "Something went wrong. Try again!");
                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos downloaded");

                    GetArchivedUserPhotos();
                }
            }
        }

        public void Preview(int selectedIndex)
        {
            new PhotoDetailsWindow(Photos, selectedIndex).Show();
        }
    }
}