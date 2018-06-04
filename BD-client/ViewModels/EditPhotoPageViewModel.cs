using BD_client.Data.Utils;
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
    class EditPhotoPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        public ObservableCollection<Domain.Category> Categories { get; set; }
        public ICommand CancelCmd { get; set; }
        public ICommand EditCmd { get; set; }
        private IDialogCoordinator dialogCoordinator;
        public ObservableCollection<Photo> Photos { get; set; }
        private int _selectedIndex;
        private string _page;
        private string _description;
        private string _tags;
        private int _selectedCategory;
        private bool _isChecked;

        public EditPhotoPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            SelectedCategory = 0;
            Photos = new ObservableCollection<Photo>();
            Categories = new ObservableCollection<Domain.Category>();
            CancelCmd = new RelayCommand(x => Cancel());
            EditCmd = new RelayCommand(x => Edit());
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

            ShowPhotosDetails();
        }

        private void ShowPhotosDetails()
        {
            Tags = "";
            if (Photos[SelectedIndex].Tags != null)
            {
                for (int i = 0; i < Photos[SelectedIndex].Tags.Count; i++)
                {
                    Tags += Photos[SelectedIndex].Tags[i].Name;
                    if(i+1 != Photos[SelectedIndex].Tags.Count)
                        Tags += " ";
                }
            }

            Description = "";
            if (Photos[SelectedIndex].Description != null)
            {
                Description = Photos[SelectedIndex].Description;
            }
        }

        private void Cancel()
        {
            MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private bool DeleteAllTags(long photoID)
        {
            string url = "/tags/all/" + photoID;
            try
            {
                ApiRequest.Delete(url);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private void EditSinglePhoto()    
        {
            string url = "/photos/" + Photos[SelectedIndex].Id;
            string[] jsonTags = Tags.Split(' ');
            string json;

            //Put photos
            var valuesPhoto = new Dictionary<string, string>
            {
                { "description", Description }
            };

            try
            {
                json = JsonConvert.SerializeObject(valuesPhoto, Formatting.Indented);
                ApiRequest.Put(url, json);
            }
            catch (Exception)
            {
                throw new Exception();
            }

            //Delete old tags
            if (!DeleteAllTags(Photos[SelectedIndex].Id))
                throw new Exception();
            //Post tags
            url = "/tags";
            List<TagDTO> tagList = new List<TagDTO>();
            for (int i = 0; i < jsonTags.Length; i++)
            {
                TagDTO newTag = new TagDTO();
                newTag.name = jsonTags[i];
                newTag.photo = Photos[SelectedIndex].Id;
                tagList.Add(newTag);
            }
            try
            {
                if (tagList.Count != 0)
                {
                    json = JsonConvert.SerializeObject(tagList, Formatting.Indented);
                    ApiRequest.Post(url, json);
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async void Edit()
        {

            try
            {
                if (!IsChecked)
                {
                    EditSinglePhoto();
                    await dialogCoordinator.ShowMessageAsync(this, "Success", "Photo was edited");
                }
                else
                {
                    List<int> photoIndex = EditMultiplePhotos();
                    await dialogCoordinator.ShowMessageAsync(this, "Result", photoIndex.Count + " of " + Photos.Count + " photos was edited");
                }
            }
            catch (Exception)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Error", "Editing failed");
            }
            MainWindow.MainVM.Page = "Pages/MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;

        }

        private List<int> EditMultiplePhotos()
        {
            var photoIndex = new List<int>();
            for (int index = 0; index < Photos.Count; index++)
            {
                string url = "/photos/" + Photos[index].Id;
                string[] jsonTags = Tags.Split(' ');
                string json;

                //Put photos
                var valuesPhoto = new Dictionary<string, string>
                {
                    { "description", Description }
                };

                try
                {
                    json = JsonConvert.SerializeObject(valuesPhoto, Formatting.Indented);
                    ApiRequest.Put(url, json);
                }
                catch (Exception)
                {
                    return photoIndex;
                }

                //Delete old tags
                if (!DeleteAllTags(Photos[index].Id))
                    return photoIndex;

                //Post tags
                url = "/tags";
                List<TagDTO> tagList = new List<TagDTO>();
                for (int i = 0; i < jsonTags.Length; i++)
                {
                    TagDTO newTag = new TagDTO();
                    newTag.name = jsonTags[i];
                    newTag.photo = Photos[index].Id;
                    tagList.Add(newTag);
                }

                try
                {
                    if (tagList.Count != 0)
                    {
                        json = JsonConvert.SerializeObject(tagList, Formatting.Indented);
                        ApiRequest.Post(url, json);
                    }
                }
                catch (Exception)
                {
                    return photoIndex;
                }

                photoIndex.Add(index);
            }
            return photoIndex;
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

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        public string Tags
        {
            get
            {
                return _tags;
            }
            set
            {
                _tags = value;
                OnPropertyChanged("Tags");
            }
        }


        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (SetField(ref _selectedIndex, value, "SelectedIndex"))
                    ShowPhotosDetails();
            }
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                if (SetField(ref _isChecked, value, "IsChecked"))
                    IsCheckedTrigger();
            }
        }

        private void IsCheckedTrigger()
        {
            if(IsChecked)
            {
                Tags = "";
                Description = "";
            }
            else
            {
                for (int i = 0; i < Photos[SelectedIndex].Tags.Count; i++)
                {
                    Tags += Photos[SelectedIndex].Tags[i].Name;
                    if(i+1!= Photos[SelectedIndex].Tags.Count)
                        Tags += " ";
                }

                Description = Photos[SelectedIndex].Description;
            }
        }

        public int SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
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