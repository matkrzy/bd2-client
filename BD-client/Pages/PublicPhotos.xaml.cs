using BD_client.Services;
using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using BD_client.Enums;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for PublicPhotos.xaml
    /// </summary>
    public partial class PublicPhotos : Page
    {
        private int CurrentPage;
        private readonly int PhotosPerPage;
        public int PhotosCount { get; set; }
        public PublicPhotosPageViewModel ViewModel;


        public PublicPhotos()
        {
            InitializeComponent();
            ViewModel = new PublicPhotosPageViewModel(DialogCoordinator.Instance,this);
            DataContext = ViewModel;


            TabControl.SelectionChanged += async (s, e) => ViewModel.OnTabSelectionChanged(s, e);
            PreviousButton.IsEnabled = false;
            PreviousButton.Click += async (s, e) => OnProceedClick(false);
            NextButton.Click += async (s, e) => OnProceedClick(true);

            CurrentPage = 0;
//            CurrentTab = null;
            PhotosPerPage = int.Parse(ConfigurationManager.AppSettings["PhotosPerPage"]);
        }

       
        private async Task OnProceedClick(bool next)
        {
//            if (next)
//            {
//                PreviousButton.IsEnabled = true;
//                CurrentPage++;
//                if (!(await CheckIfThereAreMoreToDisplay()))
//                {
//                    //Jeżeli nie ma więcej zdjęc do wyświetlenia, zablokuj przycisk
//                    NextButton.IsEnabled = false;
////                }
//            }
//            else
//            {
//                CurrentPage--;
//                //Jeżeli się cofnęliśmy to oznacza ze można przejść do przodu z powrotem
//                NextButton.IsEnabled = true;
//                if (CurrentPage == 0)
//                {
//                    //Nie ma już nic wcześniej
//                    PreviousButton.IsEnabled = false;
//                }
//            }
//            await GetPublicPhotos();

            //Przescrolluj wszystko do góry
            //HotPhotosListBox.SelectedItem = HotPhotosListBox.Items[0];

//            HotPhotosListBox.ScrollIntoView(HotPhotosListBox.Items[0]);
        }

//        private async Task<bool> CheckIfThereAreMoreToDisplay()
//        {
//            var totalCount = await PhotoService.GetPhotosCount(true);
//            //var previousPage = (CurrentPage - 1) < 0 ? 0 : CurrentPage - 1;
//            var displayedPhotosCount = CurrentPage * PhotosPerPage + PhotosPerPage;
//            return totalCount > displayedPhotosCount;
//        }
    }
}
