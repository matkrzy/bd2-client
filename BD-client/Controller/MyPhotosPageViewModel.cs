using BD_client.Common;
using BD_client.Domain;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BD_client.Api.Core;
using BD_client.Api.Utils;
using BD_client.Pages;
using Newtonsoft.Json;
using RestSharp;

namespace BD_client.ViewModels
{
    public class MyPhotosPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        private string _page;
        public NotifyTaskCompletion<List<Photo>> Photos { get; set; }

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

        public MyPhotosPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            this.Photos = new NotifyTaskCompletion<List<Photo>>(this.GetAllUserPhotos());
        }
        

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private async Task<List<Photo>> GetAllUserPhotos()
        {
            IRestResponse response = await new Request("/photos").DoGet();
            
            return JsonConvert.DeserializeObject<List<Photo>>(response.Content);
        }
    }
}