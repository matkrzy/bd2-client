using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using BD_client.Api.Core;
using BD_client.Dialogs.Share;
using BD_client.Dto;
using BD_client.Enums;
using BD_client.Windows;
using Newtonsoft.Json;
using RestSharp;
using BD_client.Pages;

namespace BD_client.ViewModels
{
    public class MyPhotosPageViewModel : INotifyPropertyChanged
    {
        public IDialogCoordinator dialogCoordinator;
        public event PropertyChangedEventHandler PropertyChanged = null;

        public List<Photo> _Photos { get; set; }

        public List<Photo> Photos
        {
            get { return _Photos; }
            set
            {
                _Photos = value;
                OnPropertyChanged("Photos");
            }
        }

        public MyPhotosPageViewModel(IDialogCoordinator instance)
        {
            MainWindow.MainVM.AssignSearchAction(this.GetAllUserPhotos);
            dialogCoordinator = instance;
            this.GetAllUserPhotos();
        }

        public void Preview(int selectedIndex)
        {
            new PhotoDetailsWindow(new ObservableCollection<Photo>(Photos), selectedIndex).Show();
        }

        public async void ArchivePhoto(List<Photo> photos)
        {
            bool errorOccurred = false;
            var progressBar = await dialogCoordinator.ShowProgressAsync(this, "Archiving", "Starting archiving");

            for (int i = 0; i < photos.Count; i++)
            {
                Photo photo = photos[i];
                photo.CategoryIds = photo.Categories.Select(c => c.Id).ToList();
                photo.Categories = null;
                photo.UserId = photo.User.Id;
                photo.User = null;

                progressBar.SetTitle($"Archiving {i + 1} of {photos.Count}");
                progressBar.SetMessage($"Archiving {photo.Name}");
                progressBar.SetProgress((double) (i + 1) / photos.Count);

                photo.PhotoState = PhotoState.ARCHIVED;
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
                await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos archived");

                GetAllUserPhotos();
            }
        }

        public async void RemovePhotos(List<Photo> photos)
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

                GetAllUserPhotos();
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
                    progressBar.SetProgress((double) (i + 1) / photos.Count);

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

                    GetAllUserPhotos();
                }
            }
            else
            {
                await progressBar.CloseAsync();
            }
        }

        private async void GetAllUserPhotos()
        {
            long userId = MainWindow.MainVM.User.Id;
            string path = $"/users/{userId}/photos";
            string searchString = MainWindow.MainVM.SearchString;

            if (searchString != null)
            {
                path += searchString;
            }

            IRestResponse response = await new Request(path).DoGet();
            this.Photos = JsonConvert.DeserializeObject<List<Photo>>(response.Content);
        }

        public async void ShareDialog(List<Photo> photos)
        {
            var customDialog = new CustomDialog() {Title = "Select share type"};
            var dataContext = new ShareDialog();
            dataContext.SetMessage($"You can share {photos.Count} photos via e-mail and make it public");

            ShareDialogTemplate template = new ShareDialogTemplate(
                    instance =>
                    {
                        this.Share(photos, dataContext);
                        dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    },
                    instance => { dialogCoordinator.HideMetroDialogAsync(this, customDialog); })
                {DataContext = dataContext};

            customDialog.Content = template;
            await dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        public async void Share(List<Photo> photos, ShareDialog context)
        {
            bool errorOccurred = false;
            foreach (var photo in photos)
            {
                if (context.Email != null)
                {
                    IRestResponse response =
                        await new Request("/photos/" + photo.Id + "/shares").DoPost(
                            new
                            {
                                photoId = photo.Id,
                                userEmail = context.Email
                            }
                        );

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        errorOccurred = true;
                    }
                }

                if (context.MakePublic)
                {
                    photo.ShareState = PhotoVisibility.PUBLIC;
                    photo.CategoryIds = photo.Categories.Select(c => c.Id).ToList();
                    photo.Categories = null;
                    photo.UserId = photo.User.Id;
                    photo.User = null;

                    IRestResponse response = await new Request($"/photos/{photo.Id}").DoPut(photo);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        errorOccurred = true;
                    }
                }
            }

            if (errorOccurred)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Oooppss", "Something went wrong. Try again!");
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos shared");
            }
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}