using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace BD_client.Models
{
    /// <summary>
    /// Exif metadata
    /// </summary>
    public class ExifMetadata
    {
        public List<Tag> ExifIFD0 { get; set; }
        public List<Tag> ExifSubIFD { get; set; }

        public ExifMetadata(string path)
        {

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var directories = ImageMetadataReader.ReadMetadata(path);

                foreach (var directory in directories)
                {
                    if (directory.Name == "Exif IFD0")
                        ExifIFD0 = new List<Tag>(directory.Tags);
                    if (directory.Name == "Exif SubIFD")
                        ExifSubIFD = new List<Tag>(directory.Tags);

                }

            }

        }

    }
}
