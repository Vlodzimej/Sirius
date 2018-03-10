using System;

namespace Sirius.Models
{
    /// <summary>
    /// Категории наименований (предметов)
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}
