using System;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls.Dialogs;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Enums;
using BD_client.Models;
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


        public int SelectedPhoto { get; set; }

        public PublicPhotos Template { get; set; }

        public PublicPhotosPageViewModel(IDialogCoordinator instance, object template)
        {
            dialogCoordinator = instance;
            Template = (PublicPhotos) template;

            LikeCmd = new RelayCommand(async x => await Like());
            Template.TabControl.SelectedIndex = 0;
//            this.OnTabSelectionChanged(null, null);

//            this.GetPhotos();
        }

        public async void GetPhotos()
        {
            Request request = new Request($"/photos");
            request.AddParameter("orderBy", "asd");
            request.AddParameter("sort", "desc");

            IRestResponse response = await request.DoGet();

            this.Photos = JsonConvert.DeserializeObject<ObservableCollection<Photo>>(response.Content);
        }

        private async Task Like()
        {
            var resAdd = await PhotoService.AddRate(Photos[SelectedPhoto].Id);
            if (resAdd)
            {
                await dialogCoordinator.ShowMessageAsync(this, "New like", "You ♥ this photo");
//                Photos[SelectedPhoto].Rate++;
            }
            else
            {
                var resRemove = await PhotoService.RemoveRate(Photos[SelectedPhoto].Id);
                if (resRemove)
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Unliked", "You disliked this photo");
//                    Photos[SelectedPhoto].Rate--;
                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", "Error occured");
                }
            }

//            Photos.Update();
        }

        public async Task OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
//           TODO: fix it 
//            Request request = new Request($"/photos");
//            String orderBy = "";
//            String sort = "";
//
//            var tab = (PublicPhotoType) Template.TabControl.SelectedIndex;
//
//            switch (tab)
//            {
//                case PublicPhotoType.Hot:
//                    orderBy = "hot_criteria";
//                    sort = "hot_sort";
//                    break;
//                case PublicPhotoType.Fresh:
//                    orderBy = "fresh_criteria";
//                    sort = "fresh_sort";
//                    break;
//                case PublicPhotoType.Trending:
//                    orderBy = "trending_criteria";
//                    sort = "trending_sort";
//                    break;
//            }
//
//            request.AddParameter("orderBy", orderBy);
//            request.AddParameter("sort", sort);
//
//            IRestResponse response = await request.DoGet();
//            this.Photos = JsonConvert.DeserializeObject<ObservableCollection<Photo>>(response.Content);

          
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}