using BD_client.Services;
using System.Collections.Generic;
using System.Linq;
using BD_client.Dto;
using BD_client.Models;

namespace BD_client.ViewModels.Windows
{
    public class PhotoDetailsWindowViewModel
    {
        public Photo Photo { get; set; }
        public ExifMetadata ExifMetadata { get; set; }
        public List<MetadataExtractor.Tag> ExifList { get; set; }
        public PhotoDetailsWindowViewModel(Photo photo)
        {
            Photo = photo;
            ExifMetadata = ImageService.GetPhotoMetadata(photo.Path);
            if (ExifMetadata.ExifIFD0 != null && ExifMetadata.ExifSubIFD != null)
            {
                ExifList = ExifMetadata.ExifIFD0.Concat(ExifMetadata.ExifSubIFD).ToList();
            }
        }
    }
}
