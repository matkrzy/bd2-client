using System.Collections.Generic;
using BD_client.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Models;
using RestSharp;
using static Newtonsoft.Json.JsonConvert;

namespace BD_client.ViewModels
{
    public class CategoriesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<GroupedCategory> _groupedCategories;

        public List<Category> Categories { get; set; }

        public List<GroupedCategory> GroupedCategories
        {
            get { return _groupedCategories; }
            set
            {
                _groupedCategories = value;
                OnPropertyChanged("GroupedCategories");
            }
        }

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

        public ICommand GetPhotosCmd { get; set; }


        public CategoriesPageViewModel()
        {
            GetPhotosCmd = new RelayCommand(GetPhotos);
            this.GetCategories();
        }

        public async void GetCategories()
        {
            string userId = ConfigurationManager.AppSettings["Id"];
            IRestResponse response = await new Request($"/users/{userId}/categories").DoGet();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Categories = DeserializeObject<List<Category>>(response.Content);
                this.PrepareCategories(Categories);
            }
        }

        public void PrepareCategories(List<Category> categories, int? parentID = null)
        {
            var groups = categories
                .ToLookup(x => x.ParentId, x => new GroupedCategory()
                {
                    Name = x.Name,
                    Id = (int) x.Id,
                    ParentId = x.ParentId
                });

            foreach (var item in groups.SelectMany(x => x))
            {
                item.Categories = groups[item.Id].ToList();
            }

            this.GroupedCategories = groups[null].ToList();
        }

        public async void GetPhotos(object param)
        {
            GroupedCategory selected = (GroupedCategory) param;

            IRestResponse response = await new Request($"/photos?categoryIds={selected.Id}").DoGet();

            if (response.StatusCode == HttpStatusCode.OK)
                this.Photos = DeserializeObject<List<Photo>>(response.Content);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}