using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Configuration;
using System.IO;
using System.Net;
using BD_client.Services;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Pages;
using MahApps.Metro.Controls.Dialogs;
using RestSharp;


namespace BD_client.ViewModels
{
    public class MainWindowViewModel :INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged = null;
        private IDialogCoordinator dialogCoordinator;

        public ICommand ProfileCmd { get; set; }
        public ICommand HelpCmd { get; set; }
        public ICommand LogoutCmd { get; set; }
        public ICommand MyPhotosCmd { get; set; }
        public ICommand PublicPhotosCmd { get; }
        public ICommand ArchivedPhotosCmd { get; }
        public ICommand CategoriesCmd { get; }

        public List<int> List { get; set; } = null;
        public List<Photo> Photos { get; set; } = null;

        private bool _enabled;

        private int _selectedIndex;
        private String _user;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public String User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        private string _page;

        public string Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public MainWindowViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;

            if (File.Exists("./token"))
            {
                this.AutoLoginAsync();
            }
            else
            {
                Page = "LogInPage.xaml";
            }

            MyPhotosCmd = new RelayCommand(x => ShowMyPhotos());
            ArchivedPhotosCmd = new RelayCommand(x => ShowArchivedPhotos());
            ProfileCmd = new RelayCommand(x => Profile());
            LogoutCmd = new RelayCommand(x => Logout());
            HelpCmd = new RelayCommand(x => Help());
            PublicPhotosCmd = new RelayCommand(x => ShowPublicPhotos());
            CategoriesCmd = new RelayCommand(x => ShowCategories());
            Photos = new List<Photo>();

        }

        private async void AutoLoginAsync()
        {
            ConfigurationManager.AppSettings["JWT"] = File.ReadAllText("./token");

            IRestResponse response = await new Request("/users/me").DoGet();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var user = Api.Utils.Utils.Deserialize<User>(response);
//                    var user = Api.Utils.Utils.Deserialize<User>(response, this, dialogCoordinator, "Something went wrong");
                    MainWindow.MainVM.User = user.Email;
                    ConfigurationManager.AppSettings["Email"] = user.Email;
                    ConfigurationManager.AppSettings["Id"] = user.Id.ToString();

                    Enabled = true;
                    User = user.Email;
                    SelectedIndex = -1;
                    Page = "MyPhotosPage.xaml";
                }
                catch (Exception e)
                {
                    Logout();
                    Page = "LogInPage.xaml";
                }
            }
            else
            {
                Page = "LogInPage.xaml";
            }
        }

        private void ShowArchivedPhotos()
        {
            MainWindow.MainVM.Page = "ArchivedPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void Help()
        {
            string pathToHtmlFile = System.IO.Directory.GetCurrentDirectory() + @"\..\..\Assets\help.html";
            System.Diagnostics.Process.Start(pathToHtmlFile);
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void ShowCategories()
        {
            MainWindow.MainVM.Page = "CategoriesPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void ShowMyPhotos()
        {
            MainWindow.MainVM.Page = "MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void ShowPublicPhotos()
        {
            MainWindow.MainVM.Page = "PublicPhotos.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void Logout()
        {
//            Response response = new Request("/account/logout").DoGet();
            String uuid = ConfigurationManager.AppSettings["uudi"];

            File.Delete("./token");
            ConfigurationManager.AppSettings["JWT"] = "";
            ConfigurationManager.AppSettings["uudi"] = "";

            ApiRequest.JWT = null;
            MainWindow.MainVM.Page = "LogInPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
            MainWindow.MainVM.Enabled = false;
            MainWindow.MainVM.User = "";
        }

        private void Profile()
        {
            MainWindow.MainVM.Page = "ProfilePage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}