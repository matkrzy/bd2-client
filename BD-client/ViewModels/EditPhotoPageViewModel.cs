﻿using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dialogs.Categories;
using BD_client.Dialogs.Share;
using BD_client.Dto;
using BD_client.Pages;
using BD_client.Services;
using RestSharp;
using MahApps.Metro.Controls.Dialogs;


namespace BD_client.ViewModels
{
    class EditPhotoPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        public ObservableCollection<Dto.Category> Categories { get; set; }
        public ICommand CancelCmd { get; set; }
        public ICommand EditCmd { get; set; }
        private IDialogCoordinator dialogCoordinator;


        public List<Photo> _Photos { get; set; }

        public List<Photo> Photos
        {
            get { return _Photos; }
            set
            {
                _Photos = value;
                OnPropertyChanged("Photos");
            }
        }


        private string _page;
        private string _description;
        private string _tags;
        public bool IsChecked { get; set; }
        public ICommand SetCategoriesCmd { get; set; }

        public EditPhotoPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            Photos = MainWindow.MainVM.Photos;
            Categories = new ObservableCollection<Dto.Category>();
            CancelCmd = new RelayCommand(x => Cancel());
            EditCmd = new RelayCommand(x => Edit());
            SetCategoriesCmd = new RelayCommand(x => SetCategories(x));
            MainWindow.MainVM.AssignSearchAction(null);
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
                photo.CategoryIds = photo.Categories.Select(c => c.Id).ToList();
                photo.Categories = null;
                photo.UserId = photo.User.Id;
                photo.User = null;

                progressBar.SetTitle($"Updating {i + 1} of {Photos.Count}");
                progressBar.SetMessage($"Updating {photo.Name}");
                progressBar.SetProgress((double) (i + 1) / Photos.Count);

                if (IsChecked)
                {
                    photo.Description = Description;
                    photo.Tags = new List<string>(Tags.Split(' '));
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

        public async void SetCategories(object param)
        {
            Photo selectedPhoto = (Photo) param;
            var customDialog = new CustomDialog() {Title = "Select categories"};

            var dataContext = new CategoriesDialog(DialogCoordinator.Instance, selectedPhoto);

            CategoriesDialogTemplate template = new CategoriesDialogTemplate(
                    instance =>
                    {
                        //close action
                        Photos = new List<Photo>(this.Photos);
                        dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    })
                {DataContext = dataContext};

            customDialog.Content = template;
            await dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}