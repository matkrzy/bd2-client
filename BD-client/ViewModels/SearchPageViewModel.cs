﻿using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.IO;
using BD_client.Dto;
using BD_client.Models;
using BD_client.Pages;
using BD_client.Services;
using BD_client.Utils;
using BD_client.Api.Core;
using RestSharp;
using System.Configuration;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace BD_client.ViewModels
{
    class SearchPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = null;
        private IDialogCoordinator dialogCoordinator;
        private string _page;
        public ObservableCollection<Dto.Category> Categories { get; set; }
        public ObservableCollection<SearchFilter> SearchFilters { get; set; }
        public ObservableCollection<Photo> PhotosResult { get; set; }
        public List<Photo> Photos { get; set; }
        public ICommand SearchCmd { get; set; }
        public ICommand CancelCmd { get; set; }
        public ICommand CategoryCmd { get; set; }
        public ICommand DescriptionCmd { get; set; }
        public ICommand TagsCmd { get; set; }
        public ICommand ExifCmd { get; set; }
        public ICommand RemovePhotoCmd { get; set; }
        public ICommand RemoveFilterCmd { get; set; }
        private int _categorySelectedIndex;
        private int _dataGridPhotoSelectedIndex;
        private int _dataGridSelectedIndex;
        private string _descriptionPhrase;
        private string _exifPhrase;
        private string _tagsPhrase;
        private string _path;
        public List<string> TagsAutocomplete { get; set; }

        public SearchPageViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
            Categories = new ObservableCollection<Dto.Category>();
            Photos = MainWindow.MainVM.Photos;
            PhotosResult = new ObservableCollection<Photo>();
            SearchFilters = new ObservableCollection<SearchFilter>();
            SearchCmd = new RelayCommand(x => ShowResults());
            CancelCmd = new RelayCommand(x => Cancel());
            CategoryCmd = new RelayCommand(x => AddCategoryFilter());
            DescriptionCmd = new RelayCommand(x => AddDescriptionFilter());
            TagsCmd = new RelayCommand(x => AddTagsFilter());
            ExifCmd = new RelayCommand(x => AddExifFilter());
            RemoveFilterCmd = new RelayCommand(x => RemoveFilter());
            RemovePhotoCmd = new RelayCommand(x => RemovePhoto());
            GetCategories();
            CategorySelectedIndex = 0;
            Path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\tmp\\own";
            TagsAutocomplete = new List<string>();
        }

        private async void GetTags()
        {
            TagsAutocomplete.Clear();
            IRestResponse response = await new Request($"/users/{ConfigurationManager.AppSettings["Id"]}/tags?q={TagsPhrase}").DoGet();
            IRestResponse response2 = await new Request($"/users/{ConfigurationManager.AppSettings["Id"]}/tags").DoGet();


            if (response.StatusCode == HttpStatusCode.OK)
            {
                ObservableCollection<Tag> tagList = JsonConvert.DeserializeObject<ObservableCollection<Tag>>(response.Content);
                foreach (var tag in tagList)
                    TagsAutocomplete.Add(tag.Name);
            }
                //            String responseContent = ApiRequest.Get(url);
                //            JsonTextReader reader = new JsonTextReader(new StringReader(responseContent));
                //            reader.SupportMultipleContent = true;
                //            List<Tag> tagsList = null;
                //            while (true)
                //            {
                //                if (!reader.Read())
                //                {
                //                    break;
                //                }
                //
                //                JsonSerializer serializer = new JsonSerializer();
                //                tagsList = serializer.Deserialize<List<Tag>>(reader);
                //
                //            }
                //
                //            foreach(var tag in tagsList)
                //            {
                //                TagsAutocomplete.Add(tag.Name);
                //            }

            }

        private void RemovePhoto()
        {
            PhotosResult.RemoveAt(DataGridPhotoSelectedIndex);
        }

        private async void GetCategories()
        {

            Request request = new Request("/users/"+ ConfigurationManager.AppSettings["Id"]+"/categories");
            IRestResponse response = await request.DoGet();
            ObservableCollection<Category> categoriesList = JsonConvert.DeserializeObject<ObservableCollection<Category>>(response.Content);

            if(categoriesList.Count!=0)
            {
                foreach(var category in categoriesList)
                {
                        bool repeated = false;
                        foreach (var displayCategory in Categories)
                        {
                            if (displayCategory.Name.ToLower().Equals(category.Name.ToLower()))
                            {
                                repeated = true;
                                break;
                            }
                        }
                        if (!repeated)
                            Categories.Add(category);

                        repeated = false;

                        }

                }
            
         

                //            string url = MainWindow.MainVM.BaseUrl + "api/v1/categories";
                //            String responseContent = ApiRequest.Get(url);
                //            JsonTextReader reader = new JsonTextReader(new StringReader(responseContent));
                //            reader.SupportMultipleContent = true;
                //            List<Dto.Category> categoriesList = null;
                //            while (true)
                //            {
                //                if (!reader.Read())
                //                {
                //                    break;
                //                }
                //
                //                JsonSerializer serializer = new JsonSerializer();
                //                categoriesList = serializer.Deserialize<List<Dto.Category>>(reader);
                //
                //            }
                //            bool repeated = false;
                //            foreach (var category in categoriesList)
                //            {
                //                foreach (var displayCategory in Categories)
                //                {
                //                    if (displayCategory.Name.ToLower().Equals(category.Name.ToLower()))
                //                    {
                //                        repeated = true;
                //                        break;
                //                    }
                //                }
                //                if (!repeated)
                //                    Categories.Add(category);
                //
                //                repeated = false;
                //            }


            }

        private void AddExifFilter()
        {
            SearchFilters.Add(new SearchFilter() { Type = "Exif", Filter = ExifPhrase });
        }

        private void RemoveFilter()
        {
            SearchFilters.RemoveAt(DataGridSelectedIndex);
        }

        private void AddTagsFilter()
        {
            SearchFilters.Add(new SearchFilter() { Type = "Tag", Filter = TagsPhrase });
        }

        private void AddDescriptionFilter()
        {
            SearchFilters.Add(new SearchFilter() { Type = "Description", Filter = DescriptionPhrase });
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

        public string DescriptionPhrase
        {
            get
            {
                return _descriptionPhrase;
            }
            set
            {
                _descriptionPhrase = value;
                OnPropertyChanged("DescriptionPhrase");
            }
        }

        public string TagsPhrase
        {
            get
            {
                return _tagsPhrase;
            }
            set
            {
                if (SetField(ref _tagsPhrase, value, "TagsPhrase")) { }
                    //TO DO: UNCOMMENT
                    GetTags();
            }
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string ExifPhrase
        {
            get
            {
                return _exifPhrase;
            }
            set
            {
                _exifPhrase = value;
                OnPropertyChanged("ExifPhrase");
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged("Path");
            }
        }

        public int CategorySelectedIndex
        {
            get
            {
                return _categorySelectedIndex;
            }
            set
            {
                _categorySelectedIndex = value;
                OnPropertyChanged("CategorySelectedIndex");
            }
        }

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

        public int DataGridPhotoSelectedIndex
        {
            get
            {
                return _dataGridPhotoSelectedIndex;
            }
            set
            {
                _dataGridPhotoSelectedIndex = value;
                OnPropertyChanged("DataGridPhotoSelectedIndex");
            }
        }


        private List<int> SearchCategories(List<int> SelectedCategoriesIds)
        {
            //List<int> photoIndex = new List<int>();
            //List<Photo> photosToDisplay = new List<Photo>();
            //List<Photo> tmpResult = null;
            //foreach (var selectedCategory in SelectedCategoriesIds)
            //{
            //    photosToDisplay.Clear();
            //    string url = MainWindow.MainVM.BaseUrl + "api/v1/photos/categories/any/" + selectedCategory;
            //    string response = ApiRequest.Get(url);
            //    var photosFromCategory = JsonConvert.DeserializeObject<List<Photo>>(response);
            //    foreach (var photo in photosFromCategory)
            //    {
            //        photosToDisplay.Add(photo);
            //    }

            //    tmpResult = Intersect(tmpResult, photosToDisplay);

            //}
            //for (int i = 0; i < Photos.Count; i++)
            //{
            //    for (int j = 0; j < tmpResult.Count; j++)
            //    {
            //        if (Photos[i].Id == tmpResult[j].Id)
            //        {
            //            photoIndex.Add(i);
            //            break;
            //        }
            //    }
            //}
            return null;
        }

        private List<int> SearchTags(string searchPhrase)
        {
            List<int> photoIndex = new List<int>();
            for (int i = 0; i < Photos.Count; i++)
            {
                if(Photos[i].Tags.Count!=0)
                {
                    var tags = Photos[i].Tags;
                    foreach (var tagName in tags)
                    {
                        if (tagName.ToLower().Contains(searchPhrase.ToLower()))
                        {
                            photoIndex.Add(i);
                        }
                    }
                }
            }

            return photoIndex;
        }

        private List<int> SearchDescription(string searchPhrase)
        {
            List<int> photoIndex = new List<int>();
            for (int i = 0; i < Photos.Count; i++)
            {
                if (searchPhrase != null)
                {
                    if (Photos[i].Description.ToLower().Contains(searchPhrase.ToLower()))
                        photoIndex.Add(i);
                }
                else
                {
                    if (Photos[i].Description == null)
                        photoIndex.Add(i);
                }
            }
            return photoIndex;
        }

        //private List<int> SearchExif()
        //{

        //}
        private List<int> GetCategoriesFilters()
        {
            List<int> searchCategories = new List<int>();
            foreach (var searchFilter in SearchFilters)
            {
                if (searchFilter.Type == "Category")
                {
                    foreach (var category in Categories)
                    {
                        if (category.Name == searchFilter.Filter)
                        {
                            searchCategories.Add(unchecked((int)category.Id));
                        }
                    }
                }
            }
            if (searchCategories.Count == 0)
                return null;
            else
                return searchCategories;
        }

        private List<string> GetExifFilters()
        {
            List<string> searchExif = new List<string>();
            foreach (var searchFilter in SearchFilters)
            {
                if (searchFilter.Type == "Exif")
                {
                    searchExif.Add(searchFilter.Filter);
                }
            }
            if (searchExif.Count == 0)
                return null;
            else
                return searchExif;
        }

        private List<string> GetDescriptionFilters()
        {
            List<string> searchDescription = new List<string>();
            foreach (var searchFilter in SearchFilters)
            {
                if (searchFilter.Type == "Description")
                {
                    searchDescription.Add(searchFilter.Filter);
                }
            }

            if (searchDescription.Count == 0)
                return null;
            else
                return searchDescription;
        }

        private List<string> GetTagFilters()
        {
            List<string> searchTag = new List<string>();
            foreach (var searchFilter in SearchFilters)
            {
                if (searchFilter.Type == "Tag")
                {
                    searchTag.Add(searchFilter.Filter);
                }
            }
            if (searchTag.Count == 0)
                return null;
            else
                return searchTag;
        }


        private List<int> GetAllPhotoIndexCategories()
        {
            List<int> searchCategories = GetCategoriesFilters();
            if (searchCategories != null)
            {
                List<int> photoIndexCategories = SearchCategories(searchCategories);
                return photoIndexCategories;
            }
            else
                return searchCategories;

        }

        private List<int> GetAllPhotoIndexTags()
        {
            List<int> resultPhotoIndex = null;
            List<string> searchTag = GetTagFilters();
            if (searchTag != null)
            {
                foreach (var tag in searchTag)
                {
                    List<int> photoIndex = SearchTags(tag);
                    resultPhotoIndex = Intersect(resultPhotoIndex, photoIndex);
                }
                return resultPhotoIndex;
            }
            else
                return null;


        }

        private List<int> GetAllPhotoIndexDescription()
        {
            List<int> resultPhotoIndex = null;
            List<string> searchDescription = GetDescriptionFilters();
            if (searchDescription != null)
            {

                foreach (var description in searchDescription)
                {
                    List<int> photoIndex = SearchDescription(description);
                    resultPhotoIndex = Intersect(resultPhotoIndex, photoIndex);
                }

                return resultPhotoIndex;
            }
            else
                return null;
        }


        private async Task<List<int>> GetAllPhotoIndexExif()
        {
            List<int> resultPhotoIndex = null;
            List<int> tmpResult = new List<int>();
            List<string> searchExif = GetExifFilters();
            if (searchExif != null)
            { 
                foreach (var exifPhrase in searchExif)
                {
                    await CheckInExif(tmpResult, exifPhrase);
                    resultPhotoIndex = Intersect(resultPhotoIndex, tmpResult);
                    tmpResult.Clear();
                }
                return resultPhotoIndex;
            }
            else
                return null;

            return null;
        }

        private async Task ReadMetadata(string path, ObservableCollection<MetadataExtractor.Tag> exif)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(path))
            using (var content = response.Content)
            using (var stream = await content.ReadAsStreamAsync())
            {
                
                try
                {
                    ObservableCollection<MetadataExtractor.Tag> ExifList = new ExifMetadata(stream).Exif;
                    //ExifList.Remove(ExifList.Single(i => i.Type == 700));
                    //ExifList.Remove(ExifList.Single(i => i.Type == 36864));
                    if (ExifList != null)
                    {
                        foreach (var ex in ExifList)
                        {
                            exif.Add(ex);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
        }


        private async Task CheckInExif(List<int> tmpResult, string exifPhrase)
        {
            for (int i = 0; i < Photos.Count; i++)
            {
                ObservableCollection<MetadataExtractor.Tag> exif = new ObservableCollection<MetadataExtractor.Tag>();
                await ReadMetadata(Photos[i].Url, exif);
                if (exif != null)
                {
                    foreach (var tag in exif)
                    {
                        if ((!string.IsNullOrEmpty(tag.Name) && tag.Name.ToLower().Contains(exifPhrase.ToLower())) ||
                            (!string.IsNullOrEmpty(tag.Description) && tag.Description.ToLower().Contains(exifPhrase.ToLower())))
                        {
                            tmpResult.Add(i);
                            break;
                        }
                    }
                }
            }

            
        }


        private async void ShowResults()
        {
            PhotosResult.Clear();
            List<int> photoIndexes = await commonPart();
            if (photoIndexes != null)
            {
                foreach (var photoIndex in photoIndexes)
                {
                    PhotosResult.Add(Photos[photoIndex]);
                }
            }
        }

        private async Task<List<int>> commonPart()
        {
            List<int> allPhotoIndexCategories = GetAllPhotoIndexCategories();
            List<int> allPhotoIndexTags = GetAllPhotoIndexTags();
            List<int> allPhotoIndexDescription = GetAllPhotoIndexDescription();
            List<int> allPhotoIndexExif = await GetAllPhotoIndexExif();
            List<int> result = null;

            result = Intersect(allPhotoIndexCategories, allPhotoIndexTags);
            result = Intersect(result, allPhotoIndexDescription);
            result = Intersect(result, allPhotoIndexExif);

            return result;
        }


        private List<T> Intersect<T>(List<T> list1, List<T> list2)
        {
            List<T> result = new List<T>();
            if (list1 == null && list2 == null)
                return null;

            if (list1 != null)
            {
                if (list1.Count != 0 && list2 == null)
                {
                    foreach (var item in list1)
                    {
                        result.Add(item);
                    }
                    return result;
                }
                if (list1.Count == 0 && list2 == null)
                    return result;
            }

            if (list2 != null)
            {
                if (list2.Count != 0 && list1 == null)
                {
                    foreach (var item in list2)
                    {
                        result.Add(item);
                    }
                    return result;
                }
                if (list2.Count == 0 && list1 == null)
                    return result;
            }


            if (list1.Count == 0 && list2.Count == 0)
            {
                foreach (var item in list1)
                {
                    result.Add(item);
                }
                return result;
            }

            if (list1.Count != 0 && list2.Count == 0)
                return result;

            if (list2.Count != 0 && list1.Count == 0)
                return result;

            foreach (var item1 in list1)
            {
                foreach (var item2 in list2)
                {
                    if (item1 is int)
                    {
                        if (IsIntEqual(Convert.ToInt32(item1), Convert.ToInt32(item2)))
                        {
                            if (!result.Contains(item1))
                                result.Add(item1);
                        }

                    }
                    if (item1 is Photo)
                    {
                        if (IsPhotoEqual((Photo)(object)item1, (Photo)(object)item2))
                        {
                            if (!result.Contains(item1))
                                result.Add(item1);
                        }
                    }

                }
            }

            return result;

        }

        private bool IsIntEqual(int item1, int item2)
        {
            if (item1 == item2)
                return true;
            else
                return false;
        }
        private bool IsPhotoEqual(Photo item1, Photo item2)
        {
            if (item1.Id == item2.Id)
                return true;
            else
                return false;
        }


        private void Cancel()
        {
            MainWindow.MainVM.Page = "MyPhotosPage.xaml";
            MainWindow.MainVM.SelectedIndex = -1;
        }

        private void AddCategoryFilter()
        {
            SearchFilters.Add(new SearchFilter() { Type = "Category", Filter = Categories[CategorySelectedIndex].Name });
        }

        virtual protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
