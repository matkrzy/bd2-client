﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Data;
using BD_client.Api.Core;
using BD_client.Dialogs.Categories;
using BD_client.Dialogs.Share;
using BD_client.Dto;
using BD_client.Pages;
using Newtonsoft.Json;
using RestSharp;

namespace BD_client.ViewModels
{
    class AddPhotosPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        public ObservableCollection<Photo> _Photos { get; set; }

        public ObservableCollection<Photo> Photos
        {
            get { return _Photos; }
            set
            {
                _Photos = value;
                OnPropertyChanged("Photos");
            }
        }

        public ICommand BrowseCmd { get; set; }
        public ICommand AddCmd { get; set; }
        public ICommand CancelCmd { get; set; }
        public ICommand RemovePhotoCmd { get; set; }
        public ICommand SetCategoriesCmd { get; set; }
        private OpenFileDialog openFileDialog;
        private IDialogCoordinator dialogCoordinator;
        private int _dataGridSelectedIndex;
        private String _page;

        public AddPhotosPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            BrowseCmd = new RelayCommand(x => Browse());
            CancelCmd = new RelayCommand(x => Cancel());
            RemovePhotoCmd = new RelayCommand(x => RemovePhoto());
            AddCmd = new RelayCommand(x => Add());
            SetCategoriesCmd = new RelayCommand(SetCategories);
            openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png",
                Multiselect = true
            };
            Photos = new ObservableCollection<Photo>();
            MainWindow.MainVM.AssignSearchAction(null);
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
                        Photos = new ObservableCollection<Photo>(this.Photos);
                        dialogCoordinator.HideMetroDialogAsync(this, customDialog);
                    })
                {DataContext = dataContext};

            customDialog.Content = template;
            await dialogCoordinator.ShowMetroDialogAsync(this, customDialog);
        }

        private void RemovePhoto()
        {
            Photos.RemoveAt(DataGridSelectedIndex);
        }

        private void Browse()
        {
            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    Photos.Add(new Photo()
                    {
                        Path = openFileDialog.FileNames[i],
                        Name = openFileDialog.SafeFileNames[i],
                        Tags = new List<string>(),
                        Categories = new List<Category>()
                    });
                }
            }
        }

        private async void Add()
        {
            bool errorOccurred = false;
            var progressBar = await dialogCoordinator.ShowProgressAsync(this, "Uploading", "Starting uploading");
            List<string> failedPhotos = new List<string>();

            for (int i = 0; i < Photos.Count; i++)
            {
                Photo photo = Photos[i];
                photo.CategoryIds = photo.Categories.Select(c => c.Id).ToList();
                photo.Categories = null;
                
                progressBar.SetTitle($"Adding {i + 1} of {Photos.Count}");
                progressBar.SetMessage($"Adding {photo.Name}");
                progressBar.SetProgress((double) (i + 1) / Photos.Count);

                Request request = new Request("/photos");
                request.AddFile(photo.Path);
                request.AddParameter("description", photo.Description);
                request.AddParameter("name", photo.Name);
                request.AddParameter("tags", photo.Tags);
                request.AddParameter("categoriesIds", photo.CategoryIds.Where(x => x != null).Cast<int>().ToList());

                IRestResponse response = await request.DoPost();

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    errorOccurred = true;
                    failedPhotos.Add(photo.Name);
                }
            }

            await progressBar.CloseAsync();


            if (errorOccurred)
            {
                foreach (string name in failedPhotos)
                {
                    try
                    {
                        Photos.Remove(Photos.Single(i => i.Name != name));
                    }
                    catch (Exception)
                    {
                    }
                }

                await dialogCoordinator.ShowMessageAsync(this, "Oooppss", "Something went wrong. Try again!");
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Success", "All photos added");
                Photos.Clear();

                MainWindow.MainVM.Page = "MyPhotosPage.xaml";
                MainWindow.MainVM.SelectedIndex = -1;
            }
        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
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

        public int DataGridSelectedIndex
        {
            get { return _dataGridSelectedIndex; }
            set
            {
                _dataGridSelectedIndex = value;
                OnPropertyChanged("DataGridSelectedIndex");
            }
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}