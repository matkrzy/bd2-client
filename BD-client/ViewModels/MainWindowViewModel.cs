using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using BD_client.Services;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Enums;
using BD_client.Pages;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using RestSharp;


namespace BD_client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        private IDialogCoordinator dialogCoordinator;
        public MainWindow template;
        public List<Photo> Photos { get; set; } = null;

        public ICommand ProfileCmd { get; set; }
        public ICommand HelpCmd { get; set; }
        public ICommand LogoutCmd { get; set; }
        public ICommand MyPhotosCmd { get; set; }
        public ICommand PublicPhotosCmd { get; }
        public ICommand ArchivedPhotosCmd { get; }
        public ICommand CategoriesCmd { get; }
        public ICommand ReportCmd { get; }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        private User _user;

        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged("User");
            }
        }

        private bool _enabled;

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

        public MainWindowViewModel(IDialogCoordinator instance, MainWindow template)
        {
            dialogCoordinator = instance;
            this.template = template;
            SelectedIndex = -1;

            ReportCmd = new RelayCommand(x => Report());
            LogoutCmd = new RelayCommand(x => Logout());
            HelpCmd = new RelayCommand(x => Help());

            MyPhotosCmd = new RelayCommand(x => NavigateToPage("MyPhotosPage.xaml"));
            ArchivedPhotosCmd = new RelayCommand(x => NavigateToPage("ArchivedPhotosPage.xaml"));
            ProfileCmd = new RelayCommand(x => NavigateToPage("ProfilePage.xaml"));
            PublicPhotosCmd = new RelayCommand(x => NavigateToPage("PublicPhotos.xaml"));
            CategoriesCmd = new RelayCommand(x => NavigateToPage("CategoriesPage.xaml"));
            Photos = new List<Photo>();

            if (File.Exists("./token"))
            {
                this.AutoLoginAsync();
            }
            else
            {
                Page = "LogInPage.xaml";
            }
        }

        private async void AutoLoginAsync()
        {
            ConfigurationManager.AppSettings["JWT"] = File.ReadAllText("./token");

            IRestResponse response = await new Request("/users/me").DoGet();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    User = Api.Utils.Utils.Deserialize<User>(response);

                    Enabled = true;
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


        private void Logout()
        {
            File.Delete("./token");
            ConfigurationManager.AppSettings["JWT"] = "";

            Page = "LogInPage.xaml";
            SelectedIndex = -1;
            Enabled = false;
            User = null;
        }

        public async void Report()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var info = await dialogCoordinator.ShowProgressAsync(this, "Generating report", "Please wait...");

                string path = dialog.SelectedPath;
                long userId = User.Id;
                string stamp = DateTime.Now.ToFileTime().ToString();
                bool status = await new Request($"/users/{userId}/report").Download(path, $"Report_{stamp}", ".pdf");


                if (status)
                {
                    await info.CloseAsync();
                    await dialogCoordinator.ShowMessageAsync(this, "Report status", "Report downloaded successfully");
                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Report status",
                        "Report download failed. Try again");
                }
            }
        }


        private void Help()
        {
            string pathToHtmlFile = System.IO.Directory.GetCurrentDirectory() + @"\..\..\Assets\help.html";
            System.Diagnostics.Process.Start(pathToHtmlFile);
            SelectedIndex = -1;
        }


        //GLOBAL METHODS

        public void NavigateToPage(string page)
        {
            Page = page;
            SelectedIndex = -1;
        }

        public void OpenSearchPanel()
        {
            bool isOpen = template.Flyout.IsOpen;
            template.Flyout.IsOpen = !isOpen;
        }

        public async void GetPhotos(PhotoVisibility visibility = PhotoVisibility.PRIVATE)
        {
            string path;
            if (visibility == PhotoVisibility.PRIVATE)
            {
                path = $"/users/{User.Id}/photos";
            }
            else
            {
                path = "/photos";
            }

            IRestResponse response = await new Request(path).DoGet();
            this.Photos = JsonConvert.DeserializeObject<List<Photo>>(response.Content);
        }

        //END GLOBAL METHODS

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}