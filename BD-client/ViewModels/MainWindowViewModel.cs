using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BD_client.Services;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Enums;
using BD_client.Models;
using BD_client.Pages;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using RestSharp;


namespace BD_client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        static CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textTransformer = cultureInfo.TextInfo;

        public event PropertyChangedEventHandler PropertyChanged = null;
        private IDialogCoordinator dialogCoordinator;
        public MainWindow template;
        public List<Photo> Photos { get; set; } = null;

        private List<Category> _categories { get; set; }

        public List<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }

        private Action SearchAction { get; set; } = null;
        public string SearchString { get; set; } = null;

        public object Visibilities { get; set; }

        public ICommand ProfileCmd { get; set; }
        public ICommand HelpCmd { get; set; }
        public ICommand LogoutCmd { get; set; }
        public ICommand MyPhotosCmd { get; set; }
        public ICommand PublicPhotosCmd { get; }
        public ICommand ArchivedPhotosCmd { get; }
        public ICommand CategoriesCmd { get; }
        public ICommand ReportCmd { get; }
        public ICommand ClearSearchFiltersCmd { get; }
        public ICommand SearchCmd { get; }


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
            ClearSearchFiltersCmd = new RelayCommand(x => ClearSearchFilters());
            SearchCmd = new RelayCommand(x => CallSearchAction());

            template.SearchButton.IsEnabled = false;
            Visibilities = Enum.GetValues(typeof(PhotoVisibility))
                .Cast<PhotoVisibility>()
                .Select(t => new
                {
                    Id = ((int) t),
                    Name = textTransformer.ToTitleCase(t.ToString().ToLower())
                }).ToList();


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
                    this.GetCategories();

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

        public async void GetCategories()
        {
            long userId = MainWindow.MainVM.User.Id;
            IRestResponse response = await new Request($"/users/{userId}/categories").DoGet();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Categories = JsonConvert.DeserializeObject<List<Category>>(response.Content);
            }
        }

        public void ClearSearchFilters()
        {
            template.nameFilter.Text = null;
            template.descriptionFilter.Text = null;
            template.tagsFilter.Text = null;
            template.creationDateFilter.SelectedDate = null;
            template.categoryFilter.SelectedValue = null;
            template.visibilityFilter.SelectedValue = null;
        }

        public void CallSearchAction()
        {
            if (this.SearchAction != null)
            {
                string name = template.nameFilter.Text;
                string description = template.descriptionFilter.Text;
                string tags = template.tagsFilter.Text;
                DateTime? creationDate = template.creationDateFilter.SelectedDate;
                Category category = (Category) template.categoryFilter.SelectedValue;
                var visibility = template.visibilityFilter.SelectedValue;

                SearchString = String.Format(
                    "?name={0}&description={1}&tags={2}&creation_date={3:d}&categoryIds={4}&visibility={5}",
                    name,
                    description,
                    tags.Length == 0 ? null : tags.Split(' '),
                    creationDate,
                    category?.Id,
                    visibility);

                this.SearchAction();
                SearchString = null;
            }
        }

        public void AssignSearchAction(Action action)
        {
            if (action != null)
            {
                this.SearchAction = action;
                template.SearchButton.IsEnabled = true;
            }
            else
            {
                template.SearchButton.IsEnabled = false;
            }
        }


        //END GLOBAL METHODS

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}