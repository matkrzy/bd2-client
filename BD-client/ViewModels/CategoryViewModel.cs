using System.Collections.Generic;
using BD_client.Dto;

namespace BD_client.Models
{
    public class GroupedCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public List<GroupedCategory> Categories { get; set; }

        public GroupedCategory(Category category)
        {
            Id = (int) category.Id;
            Name = category.Name;
            Categories = new List<GroupedCategory>();
            ParentId = category.ParentId;
        }
    }
}