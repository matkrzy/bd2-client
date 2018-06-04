using BD_client.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BD_client.Data.Photos;
using System.IO;
using BD_client.Services.Base;
using BD_client.Domain.Enums;

namespace BD_client.Services
{
    public static class PhotoService
    {
        public static async Task<List<Photo>> GetAllUserPhotos()
        {
            return await BaseService.GetAsync<List<Photo>>("/photos");
        }

        public static async Task<List<Photo>> GetUsersPhotosByCategoriesIds(bool all, params int[] categoriesIds)
        {
            var categories = string.Join(",", categoriesIds);
            var mode = all ? "all" : "any";
            var path = $"/photos/categories/{mode}/{categories}";
            return await BaseService.GetAsync<List<Photo>>(path);
        }

        public static async Task<int> AddPhoto(string name, string description, PhotoState photoState, ShareState shareState)
        {
            var body = new { name = name, description = description, photoState = photoState.ToString(), shareState = shareState.ToString() };
            var res = await ApiRequest.PostAsync("/photos", body);
            var content = await res.Content.ReadAsStringAsync();
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(content);
            //return await BaseService.PostAsync("/photos", body);
            return dictionary["id"];
        }

        public static async Task<List<Photo>> GetPublicPhotos(PublicPhotoType tab, int currentPage, int photosPerPage)
        {
            var type = tab.ToString().ToLower();
            var beg = currentPage * photosPerPage;
            var end = beg + photosPerPage;
            return await BaseService.GetAsync<List<Photo>>($"/photos/public/{type}/{beg}/{end}");

        }

        public static async Task<int> GetPhotosCount(bool arePublic)
        {
            var shareState = arePublic ? "PUBLIC" : "PRIVATE";
            var res = await ApiRequest.GetAsync($"/photos/count/{shareState}");
            var content = await res.Content.ReadAsStringAsync();
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(content);
            return dictionary["count"];
        }

    }
}
