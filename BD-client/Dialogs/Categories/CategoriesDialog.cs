using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using RestSharp;

namespace BD_client.Dialogs.Share
{
    public class CategoriesDialog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<Category> Categories { get; set; }


        public CategoriesDialog()
        {
            this.GetCategories();
        }

        public async void GetCategories()
        {
            string userId = ConfigurationManager.AppSettings["Id"];
            IRestResponse response = await new Request($"/users/{userId}/categories").DoGet();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Categories = JsonConvert.DeserializeObject<List<Category>>(response.Content);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}