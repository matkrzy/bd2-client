using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Domain;

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
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Email
        {
            get { return _email; }
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


        private void Login()
        {
            var values = new Dictionary<string, string>
            {
                {"email", Email},
                {"password", Password}
            };

            Response response = new Request("/login").DoPost(values);

            if (response.AsHttpWebResponse().StatusCode == HttpStatusCode.OK)
            {
                Cookie cookie = response.AsHttpWebResponse().Cookies[0];
                String JWT = ConfigurationManager.AppSettings["JWT"] = cookie.Value;

                var user = JsonConvert.DeserializeObject<User>(response.AsString());
                File.WriteAllText("./token", JWT);

                ConfigurationManager.AppSettings["uuid"] = user.uuid;

                MainWindow.MainVM.Enabled = true;
                MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
                MainWindow.MainVM.SelectedIndex = -1;
                MainWindow.MainVM.User = Email;
            }
            else
            {
                dialogCoordinator.ShowMessageAsync(this, "Error", "Login failed");
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