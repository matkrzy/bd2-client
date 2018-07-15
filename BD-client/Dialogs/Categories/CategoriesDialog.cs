using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BD_client.Api.Core;
using BD_client.Dto;
using BD_client.Models;
using BD_client.Utils;
using BD_client.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using RestSharp;
using TreeView = System.Windows.Controls.TreeView;

namespace BD_client.Dialogs.Share
{
    public class CategoriesDialog : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<Category> Categories { get; set; }

        private List<GroupedCategory> _groupedCategories;

        public List<GroupedCategory> GroupedCategories
        {
            get { return _groupedCategories; }
            set
            {
                _groupedCategories = value;
                OnPropertyChanged("GroupedCategories");
            }
        }

        public ICommand AddSubcategoryCmd { get; set; }
        public ICommand AddCategoryCmd { get; set; }
        public ICommand RenameCmd { get; set; }
        public ICommand RemoveCmd { get; set; }
        public ICommand AddCategoryToPhotoCmd { get; set; }
        public ICommand RemoveCategoryPhotoCmd { get; set; }

        private readonly IDialogCoordinator dialogCoordinator;

        private Photo selectedPhoto;

        public List<Category> _selectedCategories { get; set; }

        public List<Category> selectedCategories
        {
            get { return _selectedCategories; }
            set
            {
                _selectedCategories = value;
                OnPropertyChanged("selectedCategories");
            }
        }

        public CategoriesDialog()
        {
            AddSubcategoryCmd = new RelayCommand(AddSubCategory);
            AddCategoryCmd = new RelayCommand(x => AddCategory());
            RenameCmd = new RelayCommand(RenameCategory);
            RemoveCmd = new RelayCommand(RemoveCategory);
            AddCategoryToPhotoCmd = new RelayCommand(AddCategoryToPhoto);
            RemoveCategoryPhotoCmd = new RelayCommand(RemoveCategoryPhoto);

            this.GetCategories();
        }

        public CategoriesDialog(IDialogCoordinator dialogCoordinator)
        {
            AddSubcategoryCmd = new RelayCommand(AddSubCategory);
            AddCategoryCmd = new RelayCommand(x => AddCategory());
            RenameCmd = new RelayCommand(RenameCategory);
            RemoveCmd = new RelayCommand(RemoveCategory);
            AddCategoryToPhotoCmd = new RelayCommand(AddCategoryToPhoto);
            RemoveCategoryPhotoCmd = new RelayCommand(RemoveCategoryPhoto);

            this.dialogCoordinator = dialogCoordinator;

            this.GetCategories();
        }

        public CategoriesDialog(IDialogCoordinator dialogCoordinator, Photo photo)
        {
            AddSubcategoryCmd = new RelayCommand(AddSubCategory);
            AddCategoryCmd = new RelayCommand(x => AddCategory());
            RenameCmd = new RelayCommand(RenameCategory);
            RemoveCmd = new RelayCommand(RemoveCategory);
            AddCategoryToPhotoCmd = new RelayCommand(AddCategoryToPhoto);
            RemoveCategoryPhotoCmd = new RelayCommand(RemoveCategoryPhoto);

            this.dialogCoordinator = dialogCoordinator;
            this.selectedPhoto = photo;
            this.selectedCategories = new List<Category>(selectedPhoto.Categories);

            this.GetCategories();
        }

        public async void GetCategories()
        {
            string userId = ConfigurationManager.AppSettings["Id"];
            IRestResponse response = await new Request($"/users/{userId}/categories").DoGet();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Categories = JsonConvert.DeserializeObject<List<Category>>(response.Content);
                this.PrepareCategories(Categories);
            }
        }

        public void PrepareCategories(List<Category> categories, int? parentID = null)
        {
            var groups = categories
                .ToLookup(x => x.ParentId, x => new GroupedCategory()
                {
                    Name = x.Name,
                    Id = (int) x.Id,
                    ParentId = x.ParentId
                });

            foreach (var item in groups.SelectMany(x => x))
            {
                item.Categories = groups[item.Id].ToList();
            }

            this.GroupedCategories = groups[null].ToList();
        }

        public async void AddCategory()
        {
            var result = await dialogCoordinator.ShowInputAsync(this, "Add category", "Type category name");
            if (result != null)
            {
                int userId = Int32.Parse(ConfigurationManager.AppSettings["Id"]);
                Category category = new Category() {Name = result, UserId = userId};
                IRestResponse response = await new Request("/categories").DoPost(category);

                if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Category added", "Category added");
                    GetCategories();
                }
                else
                {
                    await dialogCoordinator.ShowMessageAsync(this, "Ooopppss", "Please try again");
                }
            }
        }

        public async void AddSubCategory(object param)
        {
            var result = await dialogCoordinator.ShowInputAsync(this, "Add Subcategory", "Type subcategory name");
            if (result == null)
            {
                return;
            }

            int userId = Int32.Parse(ConfigurationManager.AppSettings["Id"]);

            TreeView tree = (TreeView) param;
            GroupedCategory selected = (GroupedCategory) tree.SelectedItem;

            Category category = new Category
            {
                ParentId = selected.Id,
                UserId = userId,
                Name = result
            };

            IRestResponse response = await new Request("/categories").DoPost(category);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Subcategory added", "Subcategory added");
                this.GetCategories();
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Ooopppss", "Please try again");
            }
        }

        public async void RenameCategory(object param)
        {
            TreeView tree = (TreeView) param;
            GroupedCategory selected = (GroupedCategory) tree.SelectedItem;

            var result =
                await dialogCoordinator.ShowInputAsync(this, $"Rename {selected.Name}", "Type new category name");
            if (result == null)
            {
                return;
            }

            int userId = Int32.Parse(ConfigurationManager.AppSettings["Id"]);


            Category category = new Category
            {
                ParentId = selected.Id,
                UserId = userId,
                Name = result
            };

            IRestResponse response = await new Request($"/categories/{selected.Id}").DoPut(category);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Category updated", "Category updated");
                this.GetCategories();
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Ooopppss", "Please try again");
            }
        }

        public async void RemoveCategory(object param)
        {
            TreeView tree = (TreeView) param;
            GroupedCategory selected = (GroupedCategory) tree.SelectedItem;

            var confirm =
                await dialogCoordinator.ShowMessageAsync(this, "Are you sure?",
                    $"Are you sure that to delete {selected.Name} category?", MessageDialogStyle.AffirmativeAndNegative,
                    new MetroDialogSettings
                    {
                        AffirmativeButtonText = "OK",
                        NegativeButtonText = "CANCEL",
                        AnimateHide = true,
                        AnimateShow = true,
                    });

            if (confirm == MessageDialogResult.Negative)
            {
                return;
            }


            IRestResponse response = await new Request($"/categories/{selected.Id}").DoDelete();
            if (response.StatusCode == HttpStatusCode.Created)
            {
                await dialogCoordinator.ShowMessageAsync(this, "Category deleted", "Category deleted");
                this.GetCategories();
            }
            else
            {
                await dialogCoordinator.ShowMessageAsync(this, "Ooopppss", "Please try again");
            }
        }

        public void AddCategoryToPhoto(object param)
        {
            GroupedCategory category = (GroupedCategory) param;

            if (selectedPhoto.Categories.Find(x => x.Id == category.Id) == null)
            {
                this.selectedCategories.Add(new Category()
                {
                    Id = category.Id,
                    Name = category.Name
                });

                this.selectedCategories = new List<Category>(this.selectedCategories);
            }
        }

        public void RemoveCategoryPhoto(object param)
        {
            Category category = (Category) param;
            this.selectedCategories.RemoveAll(cat => cat.Id == category.Id);
            this.selectedCategories = new List<Category>(this.selectedCategories);
        }

        public void UpdatePhotoCategories()
        {
            this.selectedPhoto.Categories = new List<Category>(this.selectedCategories);
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}