using BD_client.Data.Photos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace BD_client.Domain
{
    public class PhotoCollection : ObservableCollection<Photo>
    {
        public List<Photo> Photos { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }
        public PhotoCollection(string path)
        {
            DirectoryInfo = new DirectoryInfo(path);
            Photos = new List<Photo>();
        }
        public PhotoCollection(string path, List<Photo> photos)
        {
            DirectoryInfo = new DirectoryInfo(path);
            Photos = photos;
            Update();
        }                
        public void Update()
        {
            ClearItems();
            foreach (var fileInfo in DirectoryInfo.GetFiles())
            {
                int photoId;
                if (int.TryParse(Path.GetFileNameWithoutExtension(fileInfo.FullName), out photoId))
                {
                    var photo = Photos.FirstOrDefault(x => x.Id == photoId);
                    if(photo != null)
                    {
                        //TODO:
                        photo.Path = fileInfo.FullName;
                        Add(photo);
                    }
                }
            }
        }
        public void Update(IEnumerable<int> exceptIds)
        {
            ClearItems();
            foreach(var fileInfo in DirectoryInfo.GetFiles())
            {
                int photoId;
                if (int.TryParse(Path.GetFileNameWithoutExtension(fileInfo.FullName), out photoId) && !exceptIds.Contains(photoId))
                {
                    Add(new Photo(fileInfo.FullName, photoId));
                }
            }

        }
        //TODO: tego sie pozbyc
        public void DisplayAll()
        {
            ClearItems();
            try
            {
                //TODO:
                foreach (var fileInfo in DirectoryInfo.GetFiles("*.jpg"))
                {
                    Add(new Photo(fileInfo.FullName, 0));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"An Error Occured: {e.Message}");
            }
        }
    }
}
