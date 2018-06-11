using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using BD_client.Dto;
using MetadataExtractor;
using Tag = MetadataExtractor.Tag;

namespace BD_client.Utils
{
    /// <summary>
    /// Exif metadata
    /// </summary>
    public class ExifMetadata
    {
        public ObservableCollection<Tag> Exif { get; set; }

        public ExifMetadata(Stream stream)
        {
            var directories = ImageMetadataReader.ReadMetadata(stream);

            foreach (var directory in directories)
            {
                if (directory.Name == "Exif IFD0")
                    Exif = new ObservableCollection<Tag>(directory.Tags);
                if (directory.Name == "Exif SubIFD")
                    Exif = new ObservableCollection<Tag>(directory.Tags);
            }
        }
    }
}