using BD_client.Data.Photos;
using BD_client.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.Services
{
    public static class ImageService
    {
        public static async Task<bool> DownloadImageToLocation(string path, int photoId)
        {
            var response = await ApiRequest.GetAsync($"/images/{photoId}");
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    await response.Content.CopyToAsync(fileStream);
                    fileStream.Close();
                    return true;
                }
                catch (Exception e)
                {
                    //
                }

            }
            return false;
        }

        public static async Task<bool> UploadImage(int id, string pathToImage, string imageName)
        {
            return await ApiRequest.PostFile($"/images/{id}", pathToImage, imageName);
        }

        public static ExifMetadata GetPhotoMetadata(string imagePath)
        {
            return new ExifMetadata(imagePath);
        }
    }
}
