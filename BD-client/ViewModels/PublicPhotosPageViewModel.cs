using System;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Enums;
using BD_client.Pages;
using BD_client.Services;
using Newtonsoft.Json;
using RestSharp;

namespace BD_client.ViewModels
{
    public class PublicPhotosPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
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

        public ICommand LikeCmd { get; set; }
        private IDialogCoordinator dialogCoordinator;


        public Photo SelectedPhoto { get; set; }

        public PublicPhotos Template { get; set; }

        public String requestPath { get; set; } = "/photos";
        public readonly int PhotosPerPage = int.Parse(ConfigurationManager.AppSettings["PhotosPerPage"]);
        public int currentPage = 0;


        public PublicPhotosPageViewModel(IDialogCoordinator instance, object template)
        {
            dialogCoordinator = instance;
            Template = (PublicPhotos) template;

            LikeCmd = new RelayCommand(async x => await Like());
        }


        private async Task Like()
        {
            int photoId = SelectedPhoto.Id;

            IRestResponse response = null;

            if (SelectedPhoto.Liked)
            {
                response = await new Request($"/photos/{photoId}/like").DoDelete();
            }
            else
            {
                response = await new Request($"/photos/{photoId}/like").DoPost();
            }


            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
            {
                this.FetchPhotos();
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", "Error occured");
            }
        }

        public async Task OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource is System.Windows.Controls.TabControl)
            {
                this.Photos = null;

                var tab = (PublicPhotoType) Template.TabControl.SelectedIndex;

                switch (tab)
                {
                    case PublicPhotoType.Hot:
                        this.requestPath = "/photos/hot";
                        currentPage = 0;
                        break;
                    case PublicPhotoType.Trending:
                        this.requestPath = "/photos/trending";
                        currentPage = 0;
                        break;
                    default:
                        this.requestPath = "/photos";
                        currentPage = 0;
                        break;
                }

                this.FetchPhotos();
            }

            e.Handled = true;
        }

        public void OnProceedClick(int step)
        {
            if (currentPage >= 0)
            {
                this.currentPage += step;
                this.FetchPhotos();
            }

        }

        public async void FetchPhotos()
        {
            Request request = new Request(this.requestPath);
            request.AddParameter("page", currentPage);
            request.AddParameter("size", PhotosPerPage);

            IRestResponse response = await request.DoGet();
            this.Photos = JsonConvert.DeserializeObject<ObservableCollection<Photo>>(response.Content);

            if (currentPage > 0)
            {
                this.Template.PreviousButton.IsEnabled = true;
            }
            else
            {
                this.Template.PreviousButton.IsEnabled = false;
            }
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}