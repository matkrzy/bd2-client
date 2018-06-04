using BD_client.Domain;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BD_client.Services;

namespace BD_client.ViewModels
{
    class SharePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;

        public ObservableCollection<Photo> Photos { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public ICommand CancelCmd { get; set; }
        public ICommand RemovePhotoCmd { get; set; }
        public ICommand ShareCmd { get; set; }
        private IDialogCoordinator dialogCoordinator;

        public int DataGridSelectedIndex
        {
            get
            {
                return _dataGridSelectedIndex;
            }
            set
            {
                _dataGridSelectedIndex = value;
                OnPropertyChanged("DataGridSelectedIndex");
            }
        }

        private string _page;
        private string _email;
        private int _dataGridSelectedIndex;


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

        public string Email
        {
            get
            {
                return _email ;
            }
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }



        public SharePageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            Photos = new ObservableCollection<Photo>();
            ShareCmd = new RelayCommand(x => Share());
            CancelCmd = new RelayCommand(x => Cancel());
            RemovePhotoCmd = new RelayCommand(x => RemovePhoto());
            if (MainWindow.MainVM.List != null)
                GetSelectedPhtotos();
        }


        private void GetSelectedPhtotos()
        {
            for (int i = 0; i < MainWindow.MainVM.List.Count; i++)
            {
                int index = MainWindow.MainVM.List[i];
                Photo newPhoto = MainWindow.MainVM.Photos[index];
                Photos.Add(newPhoto);
            }
            MainWindow.MainVM.List.Clear();
            MainWindow.MainVM.List = null;
        }

        private void RemovePhoto()
        {
            Photos.RemoveAt(DataGridSelectedIndex);
        }
        private async void Share()
        {
            List<int> photoIndex = SharePhoto();
            await dialogCoordinator.ShowMessageAsync(this, "Result", photoIndex.Count + " of " + Photos.Count + " photos was shared");
            for (int i = photoIndex.Count - 1; i >= 0; i--)
            {
                Photos.RemoveAt(photoIndex[i]);
            }
        }

        private User GetUserInfo(string email)
        {
            string url = "/users/" + email;
            String responseContent = ApiRequest.Get(url);
            User user = JsonConvert.DeserializeObject<User>(responseContent);
            return user;

        }

        private List<int> SharePhoto()
        {
            var photoIndex = new List<int>();
            User user;
            try
            {
                 user = GetUserInfo(Email);
            }
            catch (Exception)
            {
                return photoIndex;
            }

            for (int i = 0; i < Photos.Count; i++)
            {

                var valuesPhoto = new Dictionary<string, long>
                {
                    { "user", user.id },
                    {"photo", Photos[i].Id }
                };

                var json = JsonConvert.SerializeObject(valuesPhoto, Formatting.Indented);
                var photosUrl = "/shares";
                try
                {
                    ApiRequest.Post(photosUrl, json);
                    photoIndex.Add(i);
                }
                catch (Exception)
                {

                }
            }
            return photoIndex;

        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
