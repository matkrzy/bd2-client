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
        public int PhotosCount { get; set; }
        public PublicPhotosPageViewModel ViewModel;


        public PublicPhotos()
        {
            InitializeComponent();
            ViewModel = new PublicPhotosPageViewModel(DialogCoordinator.Instance, this);
            DataContext = ViewModel;


            TabControl.SelectionChanged += async (s, e) => ViewModel.OnTabSelectionChanged(s, e);
            PreviousButton.IsEnabled = false;
            PreviousButton.Click += async (s, e) => ViewModel.OnProceedClick(-1);
            NextButton.Click += async (s, e) =>ViewModel.OnProceedClick(1);

            TabControl.SelectedIndex = 0;
        }
    }
}