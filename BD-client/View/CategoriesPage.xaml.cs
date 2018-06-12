using BD_client.Common;
using BD_client.Domain;
using BD_client.Services;
using BD_client.ViewModels;
using BD_client.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BD_client.Pages
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        /// <summary>
        /// True gdy ma wyszukiwać zdjęcia będące w każdej z zaznaczonych kategorii, False gdy w jakiejkolwiek z zaznaczonych kategorii
        /// </summary>
        private bool All;
        /// <summary>
        /// Lista Id zaznaczonych kategorii
        /// </summary>
        public List<int> SelectedCategoriesIds { get; set; }
        /// <summary>
        /// ViewModel, DataContext strony
        /// </summary>
        private CategoriesPageViewModel ViewModel;
        private CategoryViewModel CategoryToMove;
        /// <summary>
        /// Konstruktor
        /// </summary>
        public CategoriesPage()
        {
//            InitializeComponent();
//            SelectedCategoriesIds = new List<int>();
//            All = true;
//            //event handlers
//            KeyDown += ResetPage;
//
//            Categories.SelectedItemChanged += async (s, e) => OnSelectCategory(s, e);
//            SearchButton.Click += async (s, e) => OnSearchButtonClick(s, e);
//            AddRootCategoryButton.Click += async (s, e) => OnAddRootCategoryButtonClick(s, e);
//
//            AddCategoryContextMenuItem.Click += async (s, e) => OnAddContextMenuButtonClick(s, e);
//            RenameCategoryContextMenuItem.Click += async (s, e) => OnRenameContextMenuButtonClick(s, e);
//            RemoveCategoryContextMenuItem.Click += async (s, e) => OnRemoveContextMenuButtonClick(s, e);
//            MoveCategoryContextMenuItem.Click += (s, e) => OnMoveContextMenuButtonClick(s, e);
//            PasteCategoryContextMenuItem.Click += async (s, e) => OnPasteContextMenuButtonClick(s, e);
//            AssignToCategoryContextMenuItem.Click += async (s, e) => OnAssignToCategoryContextMenuButtonClick(s, e);
//
//            DissociatePhotosContextMenuItem.Click += async (s, e) => OnDissociatePhotosContextMenuItemClick(s, e);
//
//            PasteCategoryContextMenuItem.IsEnabled = false;
//
//            ViewModel = new CategoriesPageViewModel();
//            ViewModel.RootCategories = new NotifyTaskCompletion<ObservableCollection<CategoryViewModel>>(GetUsersRootCategoryViewModels());
//            DataContext = ViewModel;
        }
        
    }
}