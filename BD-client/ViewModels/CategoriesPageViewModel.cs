using BD_client.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BD_client.Models;

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
