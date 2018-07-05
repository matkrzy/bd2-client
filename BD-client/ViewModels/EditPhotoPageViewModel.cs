using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Pages;
using BD_client.Services;
using RestSharp;

namespace BD_client.ViewModels
{
    class EditPhotoPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        public ObservableCollection<Dto.Category> Categories { get; set; }
        public ICommand CancelCmd { get; set; }
        public ICommand EditCmd { get; set; }
        private IDialogCoordinator dialogCoordinator;
        public List<Photo> Photos { get; set; }
        private int _selectedIndex;
        private string _page;
        private string _description;
        private string _tags;
        private int _selectedCategory;
        public bool IsChecked { get; set; }

        public EditPhotoPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            Photos = MainWindow.MainVM.Photos;
            Categories = new ObservableCollection<Dto.Category>();
            CancelCmd = new RelayCommand(x => Cancel());
            EditCmd = new RelayCommand(x => Edit());
        }

        private void Cancel()
        {
            Photos = new List<Photo>();
            MainWindow.MainVM.Page = "MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        public async void Edit()
        {
            bool errorOccurred = false;
            var progressBar = await dialogCoordinator.ShowProgressAsync(this, "Updating", "Starting updating");
            List<int> failedPhotos = new List<int>();

            for (int i = 0; i < Photos.Count; i++)
            {
                Photo photo = Photos[i];

                progressBar.SetTitle($"Updating {i + 1} of {Photos.Count}");
                progressBar.SetMessage($"Updating {photo.Name}");
                progressBar.SetProgress((double) (i + 1) / Photos.Count);

                if (IsChecked)
                {
                    photo.Description = Description;

                    //delete
                    if (photo.Tags.Count != 0)
                    {
                        photo.Tags.Clear();
                    }



                    //add
                    if (photo.TagsList != null && photo.TagsList.Length != 0)
                    {
                        Request requestTags = new Request("/tags");
                        IRestResponse responseTags = await requestTags.DoGet();
                        ObservableCollection<Tag> tags = JsonConvert.DeserializeObject<ObservableCollection<Tag>>(responseTags.Content);


                        var tagsToAdd = Tags.Split(' ');
                        for (int j = 0; j < tagsToAdd.Length; j++)
                        {
                            if (tagsToAdd[j] != "")
                            {
                                bool addToDatabase = true;
                                foreach (var tag in tags)
                                {
                                    if (tag.Name == tagsToAdd[j])
                                    {
                                        addToDatabase = false;
                                        break;
                                    }

                                }
                                photo.Tags.Add(tagsToAdd[j]);

                                if (addToDatabase)
                                {
                                    Request requestTag = new Request("/tags");
                                    requestTag.AddParameter("name", tagsToAdd[j]);
                                    IRestResponse responseTag = await requestTag.DoPost();
                                    if (responseTag.StatusCode != HttpStatusCode.OK)
                                    {
                                        errorOccurred = true;
                                        failedPhotos.Add(photo.Id);
                                    }
                                }

                            }
                        }
                    }
                }


                if (!IsChecked)
                {
                    //delete
                    if (photo.Tags.Count != 0)
                    {
                        photo.Tags.Clear();
                    }
                            


                    //add
                    if (photo.TagsList != null && photo.TagsList.Length != 0)
                    {
                        Request requestTags = new Request("/tags");
                        IRestResponse responseTags = await requestTags.DoGet();
                        ObservableCollection<Tag> tags = JsonConvert.DeserializeObject<ObservableCollection<Tag>>(responseTags.Content);
                        
  
                        var tagsToAdd = photo.TagsList.Split(' ');
                        for (int j = 0; j < tagsToAdd.Length; j++)
                        {
                            if (tagsToAdd[j] != "")
                            {
                                bool addToDatabase = true;
                                foreach (var tag in tags)
                                {
                                    if (tag.Name == tagsToAdd[j])
                                    {
                                        addToDatabase = false;
                                        break;
                                    }

                                }
                                photo.Tags.Add(tagsToAdd[j]);

                                if (addToDatabase)
                                {
                                    Request requestTag = new Request("/tags");
                                    requestTag.AddParameter("name", tagsToAdd[j]);
                                    IRestResponse responseTag = await requestTag.DoPost();
                                    if (responseTag.StatusCode != HttpStatusCode.OK)
                                    {
                                        errorOccurred = true;
                                        failedPhotos.Add(photo.Id);
                                    }
                                }

                            }
                        }
                    }
                }

                IRestResponse response = await new Request($"/photos/{photo.Id}").DoPut(photo);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    errorOccurred = true;
                    failedPhotos.Add(photo.Id);
                }
            }

            await progressBar.CloseAsync();

            if (errorOccurred)
            {
                foreach (int id in failedPhotos)
                {
                    Photos.RemoveAll(x => x.Id == id);
                }

                await dialogCoordinator.ShowMessageAsync(this, "Oooppss", "Something went wrong. Try again!");
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos updated");
                Photos.Clear();

                MainWindow.MainVM.Page = "MyPhotosPage.xaml";
                MainWindow.MainVM.SelectedIndex = -1;
            }
        }


        public string Page
        {
            get { return _page; }
            set
            {
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                OnPropertyChanged("Tags");
            }
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}