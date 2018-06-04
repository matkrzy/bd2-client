using BD_client.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD_client.Services.Base;

namespace BD_client.Services
{
    public static class CategoryService
    {
        public static async Task<List<Category>> GetUsersRootCategories()
        {
            return await BaseService.GetAsync<List<Category>>("/categories");
        }

        public static async Task<List<Category>> GetCategoryChildren(int parentId)
        {
            return await BaseService.GetAsync<List<Category>>($"/categories/{parentId}");
        }

        public static async Task<bool> AddCategory(Category category)
        {
            return await BaseService.PostAsync("/categories", category);
        }

        public static async Task<bool> DeleteCategory(int categoryId)
        {
            return await BaseService.DeleteAsync($"/categories/{categoryId}");
        }

        public static async Task<bool> EditCategory(Category category)
        {
            return await BaseService.PutAsync($"/categories/{category.Id}", category);
        }

        public static async Task<bool> AssignPhotoToCategory(int categoryId, int photoId)
        {
            var body = new { photo = photoId, category = categoryId };
            return await BaseService.PostAsync("/category", body);
        }

        public static async Task<bool> DissociatePhotoFromCategory(int categoryId, int photoId)
        {
            var body = new { photo = photoId, category = categoryId };
            return await BaseService.DeleteAsync("/category", body);
        }
    }
}
