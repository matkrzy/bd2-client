using BD_client.Domain;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BD_client.ViewModels
{
    class LogInPageViewModel : INotifyPropertyChanged
    {
        private IDialogCoordinator dialogCoordinator;
        public event PropertyChangedEventHandler PropertyChanged = null;
        public ICommand LoginCmd { get; set; }
        public ICommand RegisterCmd { get; set; }
        private String _password;
        private String _email;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }

        public LogInPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            LoginCmd = new RelayCommand(x => Login());
            RegisterCmd = new RelayCommand(x => Register());
        }

        public static void TemporaryLogin()
        {
            var url = ConfigurationManager.AppSettings["BaseApiUrl"] + "api/v1/login";
            var email = ConfigurationManager.AppSettings["Email"];
            var values = new
            {
                email = email,
                password = ConfigurationManager.AppSettings["Password"]
            };
            string json = JsonConvert.SerializeObject(values, Formatting.Indented);
            ApiRequest.Post(url, json);
        }

        private void LoginUser()
        {
            var values = new Dictionary<string, string>
            {
                { "email", Email },
                { "password", Password }
            };

            string json = JsonConvert.SerializeObject(values, Formatting.Indented);
            String url = MainWindow.MainVM.BaseUrl + "api/v1/login";
            ApiRequest.Post(url, json);
        }
        public async void Login()
        {

            try
            {
                LoginUser();
                MainWindow.MainVM.Enabled = true;
                MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
                MainWindow.MainVM.SelectedIndex = -1;
                MainWindow.MainVM.User = Email;
            }
            catch (Exception)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", "Login failed");
            }

        }

        public void Register()
        {
            MainWindow.MainVM.Page = "Pages/RegisterPage.xaml";
        }
        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
