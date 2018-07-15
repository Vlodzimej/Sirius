using System;

namespace Sirius.Models.Dtos
{
    public class ItemSaveDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid DimensionId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid VendorId { get; set; }

        public bool IsCountless { get; set; }

        public double MinimumLimit { get; set; }
    }
}
