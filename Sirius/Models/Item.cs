using System;

namespace Sirius.Models
{
    /// <summary>
    /// Наименование
    /// </summary>
    public class Item
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
        /// Идентификатор единицы измерения
        /// </summary>
        public Guid DimensionId { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public Dimension Dimension { get; set; }

        /// <summary>
        /// Идентификатор категории
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Категория
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Признак бесчисленности
        /// </summary>
        public bool IsCountless { get; set; }
        
        /// <summary>
        /// Минимальное количество по данной позиции
        /// </summary>
        public double MinimumLimit { get; set; }
    }
}
