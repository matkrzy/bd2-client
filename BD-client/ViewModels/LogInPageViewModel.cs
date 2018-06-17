using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Pages;
using RestSharp;

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


        private async void Login()
        {
            var values = new {email = Email, password = Password};

            IRestResponse response = await new Request("/login").DoPost(values);


            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var cookie = response.Cookies.ElementAt(0);

                    String JWT = ConfigurationManager.AppSettings["JWT"] = cookie.Value;

                    var user = JsonConvert.DeserializeObject<User>(response.Content);
                    File.WriteAllText("./token", JWT);

                    ConfigurationManager.AppSettings["uuid"] = user.uuid;
                    ConfigurationManager.AppSettings["Id"] = user.id.ToString();

                    MainWindow.MainVM.Enabled = true;
                    MainWindow.MainVM.Page = "MyPhotosPage.xaml";
                    MainWindow.MainVM.SelectedIndex = -1;
                    MainWindow.MainVM.User = Email;
                }
                catch (Exception e)
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Error", "Login failed");
                }
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", "Login failed");
            }
        }


        public void Register()
        {
            MainWindow.MainVM.Page = "RegisterPage.xaml";
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
