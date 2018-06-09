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
using System.Windows.Input;

namespace BD_client.ViewModels
{
    class ProfilePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        private IDialogCoordinator dialogCoordinator;
        public ICommand EditCmd { get; set; }
        private String _page;
        private String _password;
        private String _email;
        private String _name;
        private String _surname;
        private String _role;
        private long id;
        private String oldName;
        private String oldSurname;
        private String oldPassword;

        public string Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
                OnPropertyChanged("Role");
            }
        }

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


        public ProfilePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            EditCmd = new RelayCommand(x => Edit());
            try
            {
                GetUserInfo();
                oldName = Name;
                oldSurname = Surname;
                oldPassword = Password;
            }
            catch (Exception)
            {
                ShowServerError();
            }
            
        }

        private async void ShowServerError()
        {
            await dialogCoordinator.ShowMessageAsync(this, "Error", "Server error");
        }

        private void GetUserInfo()
        {
            string url = MainWindow.MainVM.BaseUrl + "api/v1/users/"+ MainWindow.MainVM.User;
            String responseContent = ApiRequest.Get(url);
            User user = JsonConvert.DeserializeObject<User>(responseContent);
            id = user.id;
            Name = user.FirstName;
            Surname = user.LastName;
            Email = user.Email;
            Password = user.Password;
            Role = user.Role.Value.ToString();

        }

        private void EditUser()
        {
            var values = new Dictionary<string, string>
            {
                { "firstName", Name },
                { "lastName", Surname },
                { "password", Password },
            };

            string json = JsonConvert.SerializeObject(values, Formatting.Indented);

            String url = MainWindow.MainVM.BaseUrl + "api/v1/users";
            ApiRequest.Put(url, json);
        }


        private async void Edit()
        {
            try
            {
                EditUser();
                await dialogCoordinator.ShowMessageAsync(this, "Success", "Profile was edited");
                MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
                MainWindow.MainVM.SelectedIndex = -1;
            }
            catch (Exception)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", "Edit failed");
                Password = oldPassword;
                Name = oldName;
                Surname = oldSurname;

            }
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
