using BD_client.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.Services.Base
{
    public static class BaseService
    {
        public static async Task<T> GetAsync<T>(string path)
        {
            var res = await ApiRequest.GetAsync(path);
            var stringifiedJson = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringifiedJson);
        }

        public static async Task<bool> PostAsync(string path, object body)
        {
            var res = await ApiRequest.PostAsync(path, body);
            return res.IsSuccessStatusCode;
        }

        public static async Task<bool> DeleteAsync(string path, object body = null)
        {
            var res = await ApiRequest.DeleteAsync(path, body);
            return res.IsSuccessStatusCode;
        }

        public static async Task<bool> PutAsync(string path, object body)
        {
            var res = await ApiRequest.PutAsync(path, body);
            return res.IsSuccessStatusCode;
        }
    }
}
