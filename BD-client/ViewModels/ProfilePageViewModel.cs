using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Pages;
using BD_client.Services;
using RestSharp;

namespace BD_client.ViewModels
{
    class ProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        private IDialogCoordinator dialogCoordinator;
        public ICommand EditCmd { get; set; }
        private String _page;

        public User _user { get; set; }

        public User user
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("user");
            }
        }


        public ProfilePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            EditCmd = new RelayCommand(x => Edit());

            this.GetUserInfo();
        }

        private async void ShowServerError()
        {
            await dialogCoordinator.ShowMessageAsync(this, "Error", "Server error");
        }

        private async void GetUserInfo()
        {
            string userId = ConfigurationManager.AppSettings["Id"];

            IRestResponse response = await new Request($"/users/{userId}").DoGet();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                user = Api.Utils.Utils.Deserialize<User>(response);
            }
        }


        private async void Edit()
        {
            IRestResponse response = await new Request($"/users/{user.id}").DoPut(this.user);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "Your profile has been updated");
                GetUserInfo();
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Oooppss", "Something went wrong. Try again!");
            }
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}