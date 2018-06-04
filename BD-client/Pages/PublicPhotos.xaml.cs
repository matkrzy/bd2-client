using BD_client.Common;
using BD_client.Domain;
using BD_client.Domain.Enums;
using BD_client.Services;
using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for PublicPhotos.xaml
    /// </summary>
    public partial class PublicPhotos : Page
    {
        private int CurrentPage;
        private PublicPhotoType CurrentTab;
        private readonly int PhotosPerPage;
        public int PhotosCount { get; set; }
        public PublicPhotosPageViewModel ViewModel;
        private string PhotoDestination;


        public PublicPhotos()
        {
            InitializeComponent();

            TabControl.SelectionChanged += async (s, e) => OnTabSelectionChanged(s, e);
            PreviousButton.IsEnabled = false;
            PreviousButton.Click += async (s, e) => OnProceedClick(false);
            NextButton.Click += async (s, e) => OnProceedClick(true);

            PhotoDestination = Directory.GetCurrentDirectory() + @"\..\..\tmp\public";
            CurrentPage = 0;
            CurrentTab = PublicPhotoType.Hot;
            PhotosPerPage = int.Parse(ConfigurationManager.AppSettings["PhotosPerPage"]);
            ViewModel = new PublicPhotosPageViewModel(DialogCoordinator.Instance, PhotoDestination);
            DataContext = ViewModel;
        }

        private async Task GetPublicPhotos()
        {
            ClearDirectory();
            //TODO: docelowo będzie tak
            var photos = await PhotoService.GetPublicPhotos(CurrentTab, CurrentPage, PhotosPerPage);
            //var photos = await PhotoService.GetAllUserPhotos();
            if (photos != null)
            {
                ViewModel.Photos.Photos.AddRange(photos);
                //TODO: loader
                //start loader
                foreach (var photo in photos)
                {
                    //TODO: różne typy zdjęć, nie jedynie .jpg
                    var completePath = $@"{PhotoDestination}\{photo.Id}.jpg";
                    if (!File.Exists(completePath))
                    {
                        // jeżeli zdjęcie nie jest jeszcze pobrane
                        if (!(await ImageService.DownloadImageToLocation(completePath, photo.Id)))
                        {
                            //TODO: wyświetlić komunikat informujący o błędzie
                        }
                    }
                }
            }
            //TODO:
            //stoploader
            ViewModel.Photos.Update();
        }

        private async Task OnProceedClick(bool next)
        {
            if (next)
            {
                PreviousButton.IsEnabled = true;
                CurrentPage++;
                if (!(await CheckIfThereAreMoreToDisplay()))
                {
                    //Jeżeli nie ma więcej zdjęc do wyświetlenia, zablokuj przycisk
                    NextButton.IsEnabled = false;
                }
            }
            else
            {
                CurrentPage--;
                //Jeżeli się cofnęliśmy to oznacza ze można przejść do przodu z powrotem
                NextButton.IsEnabled = true;
                if(CurrentPage == 0)
                {
                    //Nie ma już nic wcześniej
                    PreviousButton.IsEnabled = false;
                }
            }
            await GetPublicPhotos();

            //Przescrolluj wszystko do góry
            //HotPhotosListBox.SelectedItem = HotPhotosListBox.Items[0];

            HotPhotosListBox.ScrollIntoView(HotPhotosListBox.Items[0]);
        }

        private async Task OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentTab = (PublicPhotoType)TabControl.SelectedIndex;
            CurrentPage = 0;
            PreviousButton.IsEnabled = false;
            await GetPublicPhotos();
            NextButton.IsEnabled = await CheckIfThereAreMoreToDisplay();
        }

        private async Task<bool> CheckIfThereAreMoreToDisplay()
        {
            var totalCount = await PhotoService.GetPhotosCount(true);
            //var previousPage = (CurrentPage - 1) < 0 ? 0 : CurrentPage - 1;
            var displayedPhotosCount = CurrentPage * PhotosPerPage + PhotosPerPage;
            return totalCount > displayedPhotosCount;
        }

        private void ClearDirectory()
        {
            //usunięcie wszystkich publicznych zdjęć z folderu
            var directoryInfo = new DirectoryInfo(PhotoDestination);
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            ViewModel.Photos.Photos.Clear();
        }


    }
}
