using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace BD_client.Data.Photos
{
    /// <summary>
    /// Exif metadata
    /// </summary>
    public class ExifMetadata
    {
        //TODO: add more properties
        public double? Width { get; set; }
        public double? Height { get; set; }
        public string Date { get; set; }
        public ReadOnlyCollection<string> Authors { get; set; }
        public string CameraManufacturer { get; set; }
        public string CameraModel { get; set; }
        public string Comment { get; set; }
        public string Copyright { get; set; }
        public string Format { get; set; }
        public ReadOnlyCollection<string> Keywords { get; set; }
        public string Location { get; set; }
        public int? Rating { get; set; }
        public string Title { get; set; }
        public string ApplicationName { get; set; }

        public ExifMetadata(string path)
        {

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapSource img = BitmapFrame.Create(fs);

                Width = img.Width;
                Height = img.Height;
                BitmapMetadata md = (BitmapMetadata)img.Metadata;
                Date = md.DateTaken;
                Authors = md.Author;
                ApplicationName = md.ApplicationName;
                CameraManufacturer = md.CameraManufacturer;
                CameraModel = md.CameraModel;
                Comment = md.Comment;
                Copyright = md.Copyright;
                Format = md.Format;
                Location = md.Location;
                Keywords = md.Keywords;
                Rating = md.Rating;
                Title = md.Title;
                img = null;
            }

        }

    }
}
