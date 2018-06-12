using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using BD_client.Services.Base;
using BD_client.Dto;
using BD_client.Enums;
using System.Net.Http;
using System.Net;

namespace BD_client.Services
{
    public static class PhotoService
    {
        public static async Task<List<Photo>> GetAllUserPhotos()
        {
            return await BaseService.GetAsync<List<Photo>>("api/v1/photos");
        }

        public static async Task<List<Photo>> GetUsersPhotosByCategoriesIds(bool all, params int[] categoriesIds)
        {
            var categories = string.Join(",", categoriesIds);
            var mode = all ? "all" : "any";
            var path = $"api/v1/photos/categories/{mode}/{categories}";
            return await BaseService.GetAsync<List<Photo>>(path);
        }

        public static async Task<int> AddPhoto(string name, string description, PhotoState photoState, PhotoVisibility shareState)
        {
            var body = new { name = name, description = description, photoState = photoState.ToString(), shareState = shareState.ToString() };
            var res = await ApiRequest.PostAsync("api/v1/photos", body);
            var content = await res.Content.ReadAsStringAsync();
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(content);
            //return await BaseService.PostAsync("api/v1/photos", body);
            return dictionary["id"];
        }

        public static async Task<List<Photo>> GetPublicPhotos(PublicPhotoType tab, int currentPage, int photosPerPage)
        {
            var type = tab.ToString().ToLower();
            var beg = currentPage * photosPerPage;
            var end = beg + photosPerPage;
            return await BaseService.GetAsync<List<Photo>>($"api/v1/photos/public/{type}/{beg}/{end}");

        }

        public static async Task<List<Photo>> GetArchivedUserPhotos()
        {
            return await BaseService.GetAsync<List<Photo>>("api/v1/photos/archived");
        }

        public static async Task<int> GetPhotosCount(bool arePublic)
        {
            var shareState = arePublic ? "Public" : "Private";
            var res = await ApiRequest.GetAsync($"api/v1/photos/count/{shareState}");
            var content = await res.Content.ReadAsStringAsync();
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(content);
            return dictionary["count"];
        }

        public static async Task<bool> AddRate(long photoId)
        {
            return await BaseService.PostAsync($"api/v1/rates/{photoId}",null);
        }

        public static async Task<bool> RemoveRate(long photoId)
        {
            return await BaseService.DeleteAsync($"api/v1/rates/{photoId}");
        }

        public static async Task<bool> ChangePhotoState(PhotoState photoState, long photoId)
        {
            var body = new { photoState = photoState.ToString()};
            return await BaseService.PutAsync($"api/v1/photos/{photoId}", body);
        }

    }
}
