using BD_client.Common;
using BD_client.Data.Photos;
using BD_client.Domain;
using BD_client.Services;
using BD_client.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BD_client.ViewModels
{
    public class CategoriesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public PhotoCollection Photos { get; set; }
        public NotifyTaskCompletion<ObservableCollection<CategoryViewModel>> RootCategories { get; set; }

        public CategoriesPageViewModel()
        {
            var path = System.IO.Directory.GetCurrentDirectory() + @"\..\..\tmp\own";
            Photos = new PhotoCollection(path);
        }



    }
}
