using System;
using System.ComponentModel.DataAnnotations;

namespace Sirius.Models
{
    /// <summary>
    /// Единицы измерения
    /// </summary>
    public class Dimension
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
    }
}
