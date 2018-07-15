using System;

namespace Sirius.Models.Dtos
{
    public class ItemDetailDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Dimension Dimension { get; set; }

        public Category Category { get; set; }

        public Vendor Vendor { get; set; }

        public bool IsCountless { get; set; }

        public double MinimumLimit { get; set; }
    }
}
