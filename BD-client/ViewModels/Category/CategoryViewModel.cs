using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD_client.Domain;
using System.Collections.ObjectModel;

namespace BD_client.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public List<CategoryViewModel> Children { get; set; }

        public CategoryViewModel(Domain.Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Children = new List<CategoryViewModel>();
            ParentId = category.ParentId;
        }
    }
}
