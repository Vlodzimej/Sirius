using System;

namespace Sirius.Extends.Filters
{
    public class BatchFilter
    {
        public Guid CategoryId { get; set; }
        public Guid ItemId { get; set; }
        public Guid VendorId { get; set; }
        public decimal Cost { get; set; }
        public bool isDynamic { get; set; }
        //public DateTime startDate { get; set; }
        //public DateTime finishDate { get; set; }
    }
}