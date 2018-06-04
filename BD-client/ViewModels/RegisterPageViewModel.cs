using BD_client.Domain;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BD_client.Services;

namespace BD_client.ViewModels
{
    class RegisterPageViewModel : INotifyPropertyChanged
    {
        private IDialogCoordinator dialogCoordinator;
        public event PropertyChangedEventHandler PropertyChanged = null;
        public ICommand CancelCmd { get; set; }
        public ICommand RegisterCmd { get; set; }
        private String _page;
        private String _password;
        private String _email;
        private String _name;
        private String _surname;

        public string Surname
        {
            get
            {
                return _surname;
            }
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
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

        public string Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public RegisterPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            CancelCmd = new RelayCommand(x => Cancel());
            RegisterCmd = new RelayCommand(x => Register());
        }

        private void Cancel()
        {
            MainWindow.MainVM.Page= "Pages/LogInPage.xaml";
        }

        private async void Register()
        {
            try
            {
                RegisterUser();
                await dialogCoordinator.ShowMessageAsync(this, "Success", "User registered");
                MainWindow.MainVM.Page = "Pages/LogInPage.xaml";

            }
            catch (Exception)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", "Register failed");
            }
        }

        private void RegisterUser()
        {
            var values = new Dictionary<string, string>
            {
                { "firstName", Name },
                { "lastName", Surname },
                { "email", Email },
                { "password", Password },
                { "role", "USER" }
            };

            string json = JsonConvert.SerializeObject(values, Formatting.Indented);
            ApiRequest.Post("/users", json);
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
