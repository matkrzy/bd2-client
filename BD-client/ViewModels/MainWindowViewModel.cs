using BD_client.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Configuration;
using BD_client.Services;

namespace BD_client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        public ICommand ProfileCmd { get; set; }
        public ICommand HelpCmd { get; set; }
        public ICommand LogoutCmd { get; set; }
        public ICommand MyPhotosCmd { get; set; }
        public ICommand PublicPhotosCmd { get; }
        public ICommand CategoriesCmd { get; }

        public List<int> List { get; set; }
        public List<Photo> Photos { get; set; }

        private bool _enabled;

        private int _selectedIndex;
        private String _user;

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public String User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                OnPropertyChanged("Enabled");
            }
        }

        private string _page;

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

        internal LogInPageViewModel DataContext { get; private set; }

        public MainWindowViewModel()
        {
            List = null;
            MyPhotosCmd = new RelayCommand(x => ShowMyPhotos());
            ProfileCmd = new RelayCommand(x => Profile());
            LogoutCmd = new RelayCommand(x => Logout());
            HelpCmd = new RelayCommand(x => Help());
            PublicPhotosCmd = new RelayCommand(x => ShowPublicPhotos());
            CategoriesCmd = new RelayCommand(x => ShowCategories());

            var mode = ConfigurationManager.AppSettings["Mode"];

            // tryb developmentu
            if (mode.Equals("development"))
            {
                Page = "Pages/MyPhotosPage.xaml";
                LogInPageViewModel.TemporaryLogin();
                _enabled = true;
                _selectedIndex = -1;
                _user = "DEVELOPMENT";
            }
            else
            {
                Page = "Pages/LogInPage.xaml";
                Enabled = false;
                SelectedIndex = -1;
                User = "";
            }
        }

        private void Help()
        {
            MainWindow.MainVM.Page = "Pages/HelpPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void ShowCategories()
        {
            MainWindow.MainVM.Page = "Pages/CategoriesPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }
        private void ShowMyPhotos()
        {
            MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void ShowPublicPhotos()
        {
            MainWindow.MainVM.Page = "Pages/PublicPhotos.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void Logout()
        {
            ApiRequest.Post("/account/logout", null);
            ApiRequest.JWT = null;
            MainWindow.MainVM.Page = "Pages/LogInPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
            MainWindow.MainVM.Enabled = false;
            MainWindow.MainVM.User = "";
        }

        private void Profile()
        {
            MainWindow.MainVM.Page = "Pages/ProfilePage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }
    }
}
