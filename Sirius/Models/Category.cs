using System;

namespace Sirius.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid parentId { get; set; }
    }
}
