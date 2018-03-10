using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sirius.Models
{
    /// <summary>
    /// Предметы (наименования)
    /// </summary>
    public class Item
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid DimensionId { get; set; }

        [ForeignKey("DimensionId")]
        public Dimension Dimension { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public Guid VendorId { get; set; }

        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }
    }
}
