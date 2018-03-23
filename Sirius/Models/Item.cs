using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sirius.Models
{
    /// <summary>
    /// Предметы (наименования)
    /// </summary>
    public class Item
    {
        // Идентификатор
        public Guid Id { get; set; }
        // Название
        public string Name { get; set; }
        // Идентификатор единицы измерения
        public Guid DimensionId { get; set; }
        // Единица измерения
        public Dimension Dimension { get; set; }
        // Идентификатор категории
        public Guid CategoryId { get; set; }
        // Категория
        public Category Category { get; set; }
    }
}
