using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Net.Http;
using System.Web.UI;
using BD_client.Dto;
using BD_client.Utils;

namespace BD_client.ViewModels
{
    public class PhotoDetailsWindowViewModel : INotifyPropertyChanged
    {
        public PhotoDetailsWindowViewModel ViewModel;
        public event PropertyChangedEventHandler PropertyChanged = null;
        public Photo Photo { get; set; }
        public ExifMetadata ExifMetadata { get; set; }
        private ObservableCollection<MetadataExtractor.Tag> _ExifList;

        public ObservableCollection<MetadataExtractor.Tag> ExifList
        {
            get { return _ExifList; }
            set
            {
                _ExifList = value;
                OnPropertyChanged("ExifList");
            }
        }


        public PhotoDetailsWindowViewModel(Photo photo)
        {
            this.Photo = photo;
            ReadMetadata(photo.Url);
        }

        private async void ReadMetadata(string path)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(path))
            using (var content = response.Content)
            using (var stream = await content.ReadAsStreamAsync())
            {
                this.ExifList = new ExifMetadata(stream).Exif;
                try
                {
                    this.ExifList.Remove(this.ExifList.Single(i => i.Type == 700));
                    this.ExifList.Remove(this.ExifList.Single(i => i.Type == 36864));
                }
                catch (Exception e)
                {
                    //
                }
            }
        }

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}