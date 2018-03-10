using System;
using System.ComponentModel.DataAnnotations;

namespace Sirius.Models
{
    /// <summary>
    /// Единицы измерения
    /// </summary>
    public class Dimension
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
