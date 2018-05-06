using System;

namespace Sirius.Extends.Filters
{
    public class MetaFilter
    {
        public Guid CategoryId { get; set; }
        public Guid ItemId { get; set; }
        public Guid VendorId { get; set; }
        public Guid TypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
    }
}
