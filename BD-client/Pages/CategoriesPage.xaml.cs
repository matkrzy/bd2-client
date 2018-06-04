using BD_client.Common;
using BD_client.Domain;
using BD_client.Services;
using BD_client.ViewModels;
using BD_client.ViewModels.Category;
using BD_client.Windows;
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
            InitializeComponent();
            SelectedCategoriesIds = new List<int>();
            All = true;
            //event handlers
            KeyDown += ResetPage;

            Categories.SelectedItemChanged += async (s, e) => OnSelectCategory(s, e);
            SearchButton.Click += async (s, e) => OnSearchButtonClick(s, e);
            AddRootCategoryButton.Click += async (s, e) => OnAddRootCategoryButtonClick(s, e);

            AddCategoryContextMenuItem.Click += async (s, e) => OnAddContextMenuButtonClick(s, e);
            RenameCategoryContextMenuItem.Click += async (s, e) => OnRenameContextMenuButtonClick(s, e);
            RemoveCategoryContextMenuItem.Click += async (s, e) => OnRemoveContextMenuButtonClick(s, e);
            MoveCategoryContextMenuItem.Click += (s, e) => OnMoveContextMenuButtonClick(s, e);
            PasteCategoryContextMenuItem.Click += async (s, e) => OnPasteContextMenuButtonClick(s, e);
            AssignToCategoryContextMenuItem.Click += async (s, e) => OnAssignToCategoryContextMenuButtonClick(s, e);

            DissociatePhotosContextMenuItem.Click += async (s, e) => OnDissociatePhotosContextMenuItemClick(s, e);

            PasteCategoryContextMenuItem.IsEnabled = false;

            ViewModel = new CategoriesPageViewModel();
            ViewModel.RootCategories = new NotifyTaskCompletion<ObservableCollection<CategoryViewModel>>(GetUsersRootCategoryViewModels());
            DataContext = ViewModel;
        }

        private void ResetPage(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                PasteCategoryContextMenuItem.IsEnabled = false;
                CategoryToMove = null;
                SelectedCategoriesIds.Clear();
                SelectedCategories.Items.Clear();
                AddRootCategoryButton.Content = "Add";
                All = true;
                AllButton.Content = "All";
            }
        }

        /// <summary>
        /// (Handler dla eventu click na kategorii)
        /// Pobiera podkategorie danej kategorii i dołącza je do drzewa
        /// </summary>
        private async Task OnSelectCategory(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedNode = e.NewValue as CategoryViewModel;
            var children = await CategoryService.GetCategoryChildren(selectedNode.Id);

            if (children != null)
            {
                //TODO: jak Michal poprawi to usunąć parentId z konstruktora
                var childrenViewModels = children?.Select(x => new CategoryViewModel(x) { ParentId = selectedNode.Id });
                selectedNode.Children.Clear();
                selectedNode.Children.AddRange(childrenViewModels);
            }


            var treeViewItem = Categories.ItemContainerGenerator.ContainerFromItem(e.NewValue) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.Selected += CategorySelected;
            }

            if (EnableSearching.IsChecked.Value)
            {
                AddToSelectedCategories(selectedNode);
            }
            else
            {
                await ReloadCategoryPhotos(selectedNode.Id);
            }
        }
        /// <summary>
        /// Triggerowane dla kategorii nie mających potomków
        /// </summary>
        private void CategorySelected(object sender, RoutedEventArgs e)
        {
            var treeViewItem = e.OriginalSource as TreeViewItem;
            var categoryViewModel = treeViewItem.Header as CategoryViewModel;
            if (EnableSearching.IsChecked.Value)
            {
                AddToSelectedCategories(categoryViewModel);
            }
        }
        /// <summary>
        /// Dodaje zaznaczoną kategorię do listy wybranych kategorii
        /// </summary>
        /// <param name="node">Zaznaczona kategoria</param>
        private void AddToSelectedCategories(CategoryViewModel node)
        {
            if (!SelectedCategoriesIds.Contains(node.Id))
            {
                SelectedCategoriesIds.Add(node.Id);
                SelectedCategories.Items.Add(node.Name);
            }
        }
        /// <summary>
        /// Zmień tryb wyszukiwania pomiędzy All i Any
        /// </summary>
        private void OnAllButtonClick(object sender, RoutedEventArgs e)
        {
            if (All)
            {
                AllButton.Content = "Any";
            }
            else
            {
                AllButton.Content = "All";
            }
            All = !All;
        }
        /// <summary>
        /// Wyszukaj zdjęcia należące do zaznaczonych kategorii
        /// </summary>
        private async Task OnSearchButtonClick(object sender, RoutedEventArgs e)
        {
            var photosToDisplay = await PhotoService.GetUsersPhotosByCategoriesIds(All, SelectedCategoriesIds.ToArray());
            ViewModel.Photos.Photos.Clear();
            if (photosToDisplay != null)
            {
                ViewModel.Photos.Photos.AddRange(photosToDisplay);
            }
            ViewModel.Photos.Update();
        }

        private void OnPhotoDbClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void OnEditPhoto(object sender, RoutedEventArgs e)
        {

        }
        private void OnDownloadPhoto(object sender, RoutedEventArgs e)
        {

        }
        private void OnRemovePhoto(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Dodaj kategorię jako dziecko aktualnie zaznaczonej kategorii bądź jako root jeżeli żadna z kategorii nie jest zaznaczona
        /// </summary>
        private async Task OnAddContextMenuButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedNode = Categories.SelectedItem as CategoryViewModel;
            await AddCategory(selectedNode);
        }
        private async Task OnRenameContextMenuButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedNode = Categories.SelectedItem as CategoryViewModel;
            if (selectedNode != null)
            {
                var inputDialog = new InputTextModalWindow("Rename category", "Name:", selectedNode.Name);
                if (inputDialog.ShowDialog() == true)
                {
                    var categoryToEdit = new Category
                    {
                        Id = selectedNode.Id,
                        Name = inputDialog.Answer,
                        ParentId = selectedNode.ParentId
                    };
                    if (await CategoryService.EditCategory(categoryToEdit))
                    {
                        await ReloadCategories();
                    }
                }

            }



        }
        private async Task OnRemoveContextMenuButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedNode = Categories.SelectedItem as CategoryViewModel;
            if (await CategoryService.DeleteCategory(selectedNode.Id))
            {
                await ReloadCategories();
            }
        }

        private void OnMoveContextMenuButtonClick(object sender, RoutedEventArgs e)
        {
            CategoryToMove = Categories.SelectedItem as CategoryViewModel;
            PasteCategoryContextMenuItem.IsEnabled = true;
            AddRootCategoryButton.Content = "Move";
        }

        private async Task OnPasteContextMenuButtonClick(object sender, RoutedEventArgs e)
        {
            if (CategoryToMove != null)
            {
                var categoryToPasteAsChild = Categories.SelectedItem as CategoryViewModel;
                var categoryToMove = new Category
                {
                    Id = CategoryToMove.Id,
                    Name = CategoryToMove.Name,
                    ParentId = categoryToPasteAsChild.Id
                };

                if (await CategoryService.EditCategory(categoryToMove))
                {
                    CategoryToMove = null;
                    PasteCategoryContextMenuItem.IsEnabled = false;
                    await ReloadCategories();
                }
            }
        }

        private async Task OnAddRootCategoryButtonClick(object sender, RoutedEventArgs e)
        {
            // przeniesienie kategorii jako root
            if (CategoryToMove != null)
            {
                var category = new Category
                {
                    Id = CategoryToMove.Id,
                    Name = CategoryToMove.Name,
                    ParentId = null
                };
                if (await CategoryService.EditCategory(category))
                {
                    await ReloadCategories();
                    AddRootCategoryButton.Content = "Add";
                    CategoryToMove = null;
                }
            }
            else
            {
                // dodanie nowej root kategorii
                await AddCategory(null);
            }

        }

        private async Task AddCategory(CategoryViewModel selectedCategory)
        {
            var inputDialog = new InputTextModalWindow("Enter category name", "Name:");
            if (inputDialog.ShowDialog() == true)
            {
                var categoryToAdd = new Category
                {
                    Name = inputDialog.Answer,
                    ParentId = selectedCategory?.Id,
                };

                if (await CategoryService.AddCategory(categoryToAdd))
                {
                    await ReloadCategories();
                }
                else
                {
                    //TODO: wyświetlić informację o błędzie 
                }
            }
        }


        private async Task ReloadCategories()
        {
            var collection = await GetUsersRootCategoryViewModels();
            Categories.ItemsSource = collection;
        }

        private async Task<ObservableCollection<CategoryViewModel>> GetUsersRootCategoryViewModels()
        {
            try
            {
                var categories = await CategoryService.GetUsersRootCategories();
                if (categories != null)
                {
                    var list = categories.Select(x => new CategoryViewModel(x)).ToList();
                    return new ObservableCollection<CategoryViewModel>(list);
                }
                return null;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        private async Task OnAssignToCategoryContextMenuButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedNode = Categories.SelectedItem as CategoryViewModel;
            if (selectedNode != null)
            {
                var alreadyAssignedPhotosIds = ViewModel.Photos.Photos.Select(x => x.Id);
                var selectPhotosModalWindow = new SelectPhotosModalWindow(alreadyAssignedPhotosIds);
                if (selectPhotosModalWindow.ShowDialog() == true)
                {
                    var selectedPhotosIds = selectPhotosModalWindow.SelectedPhotosIds;
                    foreach (var selectedPhotoId in selectedPhotosIds)
                    {
                        if(!(await CategoryService.AssignPhotoToCategory(selectedNode.Id, selectedPhotoId)))
                        {
                            ///TODO: wyświetlić komunikat o niepowodzeniu
                        }
                    }
                    //Przeładowanie zdjęć w kategorii
                    await ReloadCategoryPhotos(selectedNode.Id);
                }
            }
        }

        private async Task ReloadCategoryPhotos(int categoryId)
        {
            var photosToDisplay = await PhotoService.GetUsersPhotosByCategoriesIds(true, categoryId);
            ViewModel.Photos.Photos.Clear();
            if (photosToDisplay != null)
            {
                ViewModel.Photos.Photos.AddRange(photosToDisplay);
            }
            ViewModel.Photos.Update();
        }

        private async Task OnDissociatePhotosContextMenuItemClick(object sender, RoutedEventArgs e)
        {
            var selectedCategory = Categories.SelectedItem as CategoryViewModel;
            var selectedPhotos = CategoryPhotosListBox.SelectedItems.Cast<Photo>();

            if(selectedCategory != null && selectedPhotos != null)
            {
                foreach (var photo in selectedPhotos)
                {
                    if (!(await CategoryService.DissociatePhotoFromCategory(selectedCategory.Id, photo.Id)))
                    {
                        ///TODO: wyświetlić komunikat o niepowodzeniu
                    }
                }
                await ReloadCategoryPhotos(selectedCategory.Id);
            }           
        }
    }
}