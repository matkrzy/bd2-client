using System.Collections.Generic;

namespace BD_client.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public List<CategoryViewModel> Children { get; set; }

        public CategoryViewModel(Dto.Category category)
        {
            Id = (int)category.Id;
            Name = category.Name;
            Children = new List<CategoryViewModel>();
            ParentId = category.ParentId;
        }
    }
}
