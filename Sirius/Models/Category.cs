using System;

namespace Sirius.Models
{
    /// <summary>
    /// Категории наименований (предметов)
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Родительская категория (временно не используется)
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}
